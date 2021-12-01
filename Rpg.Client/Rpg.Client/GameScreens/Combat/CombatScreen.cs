using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Rpg.Client.Core;
using Rpg.Client.Core.Skills;
using Rpg.Client.Engine;
using Rpg.Client.GameScreens.Combat.GameObjects;
using Rpg.Client.GameScreens.Combat.GameObjects.Background;
using Rpg.Client.GameScreens.Combat.Tutorial;
using Rpg.Client.GameScreens.Combat.Ui;
using Rpg.Client.GameScreens.Common;
using Rpg.Client.ScreenManagement;

namespace Rpg.Client.GameScreens.Combat
{
    internal class CombatScreen : GameScreenWithMenuBase
    {
        private const int BACKGROUND_LAYERS_COUNT = 3;
        private const float BACKGROUND_LAYERS_SPEED = 0.1f;

        public static bool _tutorial;

        private readonly AnimationManager _animationManager;
        private readonly IList<IInteractionDelivery> _bulletObjects;
        private readonly Camera2D _camera;
        private readonly IReadOnlyCollection<IBackgroundObject> _cloudLayerObjects;
        private readonly IList<CorpseGameObject> _corpseObjects;
        private readonly IDice _dice;
        private readonly IEventCatalog _eventCatalog;
        private readonly IReadOnlyList<IBackgroundObject> _foregroundLayerObjects;
        private readonly GameObjectContentStorage _gameObjectContentStorage;
        private readonly IList<UnitGameObject> _gameObjects;
        private readonly Globe _globe;
        private readonly GlobeNode _globeNode;
        private readonly GlobeProvider _globeProvider;
        private readonly IList<ButtonBase> _hudButtons;
        private readonly ResolutionIndependentRenderer _resolutionIndependentRenderer;
        private readonly ScreenShaker _screenShaker;
        private readonly IUiContentStorage _uiContentStorage;

        private readonly Vector2[] _unitPredefinedPositions;
        private readonly IUnitSchemeCatalog _unitSchemeCatalog;
        private readonly Core.Combat _сombat;

        private float _bgCenterOffsetPercentage;
        private bool _bossWasDefeat;
        private double _combatFinishedCounter;

        private bool? _combatFinishedVictory;

        private bool _combatInitialized;
        private bool _combatResultModalShown;
        private CombatSkillPanel? _combatSkillsPanel;

        private bool _finalBossWasDefeat;


        private bool _interactButtonClicked;
        private UnitPanelController? _unitPanelController;

        public CombatScreen(EwarGame game) : base(game)
        {
            var soundtrackManager = Game.Services.GetService<SoundtrackManager>();
            soundtrackManager.PlayBattleTrack();

            _globeProvider = game.Services.GetService<GlobeProvider>();
            _camera = Game.Services.GetService<Camera2D>();

            _globe = _globeProvider.Globe;

            _сombat = _globe.ActiveCombat ??
                      throw new InvalidOperationException(
                          nameof(_globe.ActiveCombat) + " can't be null in this screen.");

            _globeNode = _сombat.Node;

            _gameObjects = new List<UnitGameObject>();
            _corpseObjects = new List<CorpseGameObject>();
            _bulletObjects = new List<IInteractionDelivery>();
            _hudButtons = new List<ButtonBase>();

            _gameObjectContentStorage = game.Services.GetService<GameObjectContentStorage>();
            _uiContentStorage = game.Services.GetService<IUiContentStorage>();
            _animationManager = game.Services.GetService<AnimationManager>();
            _dice = Game.Services.GetService<IDice>();

            _resolutionIndependentRenderer = Game.Services.GetService<ResolutionIndependentRenderer>();

            var bgofSelector = Game.Services.GetService<BackgroundObjectFactorySelector>();

            _unitSchemeCatalog = game.Services.GetService<IUnitSchemeCatalog>();
            _eventCatalog = game.Services.GetService<IEventCatalog>();

            var backgroundObjectFactory = bgofSelector.GetBackgroundObjectFactory(_globeNode.Sid);

            _cloudLayerObjects = backgroundObjectFactory.CreateCloudLayerObjects();
            _foregroundLayerObjects = backgroundObjectFactory.CreateForegroundLayerObjects();

            _unitPredefinedPositions = new[]
            {
                new Vector2(300, 300),
                new Vector2(200, 250),
                new Vector2(200, 350)
            };

            _screenShaker = new ScreenShaker();
        }

        protected override void DrawContent(SpriteBatch spriteBatch)
        {
            _resolutionIndependentRenderer.BeginDraw();

            DrawGameObjects(spriteBatch);

            DrawHud(spriteBatch);
        }

        protected override void UpdateContent(GameTime gameTime)
        {
            base.UpdateContent(gameTime);

            if (!_tutorial)
            {
                _tutorial = true;
                var tutorialModal = new TutorialModal(new CombatTutorialPageDrawer(_uiContentStorage),
                    _uiContentStorage, _resolutionIndependentRenderer);
                AddModal(tutorialModal, isLate: false);
            }

            if (!_combatInitialized)
            {
                CombatInitialize();
                _combatInitialized = true;
            }
            else
            {
                UpdateBackgroundObjects(gameTime);

                HandleBullets(gameTime);

                HandleUnits(gameTime);

                if (!_сombat.Finished && _combatFinishedVictory is null)
                {
                    HandleCombatHud();
                }

                _screenShaker.Update(gameTime);
            }

            HandleBackgrounds();

            if (_combatFinishedVictory is not null)
            {
                UpdateCombatFinished(gameTime);
            }
        }

        private void Actor_SkillAnimationCompleted(object? sender, EventArgs e)
        {
            if (sender is UnitGameObject unit)
            {
                unit.SkillAnimationCompleted -= Actor_SkillAnimationCompleted;
            }

            _сombat.Update();
        }

        private static void ApplyXp(IReadOnlyCollection<XpAward> xpItems)
        {
            foreach (var item in xpItems)
            {
                item.Unit.GainXp(item.XpAmount);
            }
        }

        private void Combat_ActionGenerated(object? sender, ActionEventArgs e)
        {
            var actor = GetUnitGameObject(e.Actor);
            var target = GetUnitGameObject(e.Target);

            var blocker = _animationManager.CreateAndUseBlocker();

            actor.SkillAnimationCompleted += Actor_SkillAnimationCompleted;

            var bulletBlocker = _animationManager.CreateAndUseBlocker();

            actor.UseSkill(target, blocker, bulletBlocker, _bulletObjects, e.Skill, e.Action);
        }

        private void Combat_CombatUnitRemoved(object? sender, CombatUnit combatUnit)
        {
            var gameObject = _gameObjects.Single(x => x.CombatUnit == combatUnit);
            _gameObjects.Remove(gameObject);
            combatUnit.HasTakenDamage -= CombatUnit_HasTakenDamage;
            combatUnit.HasBeenHealed -= CombatUnit_Healed;
            combatUnit.HasAvoidedDamage -= CombatUnit_HasAvoidedDamage;
        }

        private void Combat_Finish(object? sender, CombatFinishEventArgs e)
        {
            _hudButtons.Clear();
            _combatSkillsPanel = null;

            _combatFinishedVictory = e.Victory;

            // See UpdateCombatFinished next
        }

        private void Combat_UnitChanged(object? sender, UnitChangedEventArgs e)
        {
            if (e.OldUnit != null)
            {
                var oldView = GetUnitGameObject(e.OldUnit);
                oldView.IsActive = false;
            }

            _combatSkillsPanel.Unit = null;
            _combatSkillsPanel.SelectedCard = null;
            _combatSkillsPanel.IsEnabled = false;
        }

        private void Combat_UnitDied(object? sender, CombatUnit e)
        {
            var unitGameObject = GetUnitGameObject(e);

            var corpse = unitGameObject.CreateCorpse();
            _corpseObjects.Add(corpse);
        }

        private void Combat_UnitEntered(object? sender, CombatUnit combatUnit)
        {
            var position = GetUnitPosition(combatUnit.Index, combatUnit.Unit.IsPlayerControlled);
            var gameObject =
                new UnitGameObject(combatUnit, position, _gameObjectContentStorage, _camera, _screenShaker);
            _gameObjects.Add(gameObject);
            combatUnit.HasTakenDamage += CombatUnit_HasTakenDamage;
            combatUnit.HasBeenHealed += CombatUnit_Healed;
            combatUnit.HasAvoidedDamage += CombatUnit_HasAvoidedDamage;
        }

        private void Combat_UnitHasBeenDamaged(object? sender, CombatUnit e)
        {
            var unitGameObject = GetUnitGameObject(e);

            unitGameObject.AnimateWound();
        }

        private void Combat_UnitPassed(object? sender, CombatUnit e)
        {
            var unitGameObject = GetUnitGameObject(e);
            var textPosition = GetUnitGameObject(e).Position;
            var font = _uiContentStorage.GetMainFont();

            var passIndicator = new MovePassedComponent(textPosition, font);

            unitGameObject.AddChild(passIndicator);
        }

        private void Combat_UnitReadyToControl(object? sender, CombatUnit e)
        {
            if (!e.Unit.IsPlayerControlled)
            {
                return;
            }

            if (_combatSkillsPanel is null)
            {
                return;
            }

            var selectedUnit = e;

            _combatSkillsPanel.IsEnabled = true;
            _combatSkillsPanel.Unit = selectedUnit;
            _combatSkillsPanel.SelectedCard = selectedUnit.CombatCards.First();
            var unitGameObject = GetUnitGameObject(e);
            unitGameObject.IsActive = true;
        }

        private void CombatInitialize()
        {
            _combatSkillsPanel = new CombatSkillPanel(_uiContentStorage, _сombat, _resolutionIndependentRenderer);
            _combatSkillsPanel.CardSelected += CombatSkillsPanel_CardSelected;
            _сombat.UnitChanged += Combat_UnitChanged;
            _сombat.CombatUnitReadyToControl += Combat_UnitReadyToControl;
            _сombat.CombatUnitEntered += Combat_UnitEntered;
            _сombat.CombatUnitRemoved += Combat_CombatUnitRemoved;
            _сombat.UnitDied += Combat_UnitDied;
            _сombat.ActionGenerated += Combat_ActionGenerated;
            _сombat.Finish += Combat_Finish;
            _сombat.UnitHasBeenDamaged += Combat_UnitHasBeenDamaged;
            _сombat.UnitPassedTurn += Combat_UnitPassed;
            _сombat.Initialize();
            _сombat.Update();

            _unitPanelController = new UnitPanelController(_resolutionIndependentRenderer, _сombat,
                _uiContentStorage, _gameObjectContentStorage);
        }

        private void CombatResultModal_Closed(object? sender, EventArgs e)
        {
            _animationManager.DropBlockers();

            if (sender is null)
            {
                throw new InvalidOperationException("Handler must be assigned to object instance instead static.");
            }

            var combatResultModal = (CombatResultModal)sender;

            if (combatResultModal.CombatResult == CombatResult.Victory ||
                combatResultModal.CombatResult == CombatResult.NextCombat)
            {
                var currentCombatList = _globeNode.CombatSequence.Combats.ToList();
                currentCombatList.Remove(_сombat.CombatSource);
                _globeNode.CombatSequence.Combats = currentCombatList;

                if (_сombat.Node.CombatSequence.Combats.Any())
                {
                    var nextCombat = _globeNode.CombatSequence.Combats.First();
                    _globe.ActiveCombat = new Core.Combat(_globe.Player.Party,
                        _globeNode,
                        nextCombat,
                        _сombat.Biome,
                        _dice,
                        _сombat.IsAutoplay);

                    ScreenManager.ExecuteTransition(this, ScreenTransition.Combat);
                }
                else
                {
                    RestoreGroupAfterCombat();

                    if (_bossWasDefeat)
                    {
                        if (_finalBossWasDefeat)
                        {
                            ScreenManager.ExecuteTransition(this, ScreenTransition.EndGame);
                        }
                        else
                        {
                            if (_globe.CurrentEventNode is null)
                            {
                                _globeProvider.Globe.UpdateNodes(_dice, _unitSchemeCatalog, _eventCatalog);
                                ScreenManager.ExecuteTransition(this, ScreenTransition.Biome);
                            }
                            else
                            {
                                _globeProvider.Globe.UpdateNodes(_dice, _unitSchemeCatalog, _eventCatalog);
                                ScreenManager.ExecuteTransition(this, ScreenTransition.Map);
                            }
                        }
                    }
                    else
                    {
                        if (_globe.CurrentEventNode is null)
                        {
                            _globeProvider.Globe.UpdateNodes(_dice, _unitSchemeCatalog, _eventCatalog);
                            ScreenManager.ExecuteTransition(this, ScreenTransition.Biome);
                        }
                        else
                        {
                            ScreenManager.ExecuteTransition(this, ScreenTransition.Event);
                        }
                    }
                }
            }
            else if (combatResultModal.CombatResult == CombatResult.Defeat)
            {
                RestoreGroupAfterCombat();
                _globeProvider.Globe.UpdateNodes(_dice, _unitSchemeCatalog, _eventCatalog);
                ScreenManager.ExecuteTransition(this, ScreenTransition.Biome);
            }
            else
            {
                Debug.Fail("Unknown combat result.");

                RestoreGroupAfterCombat();

                // Fallback is just show biome.
                _globeProvider.Globe.UpdateNodes(_dice, _unitSchemeCatalog, _eventCatalog);
                ScreenManager.ExecuteTransition(this, ScreenTransition.Biome);
            }
        }

        private void CombatSkillsPanel_CardSelected(object? sender, CombatSkillCard? skillCard)
        {
            RefreshHudButtons(skillCard);
        }

        private void CombatUnit_HasAvoidedDamage(object? sender, EventArgs e)
        {
            Debug.Assert(sender is not null);
            var combatUnit = (CombatUnit)sender;
            var unitGameObject = GetUnitGameObject(combatUnit);
            var textPosition = GetUnitGameObject(combatUnit).Position;
            var font = _uiContentStorage.GetMainFont();

            var passIndicator = new EvasionComponent(textPosition, font);

            unitGameObject.AddChild(passIndicator);
        }

        private void CombatUnit_HasTakenDamage(object? sender, UnitHitPointsChangedEventArgs e)
        {
            Debug.Assert(e.CombatUnit is not null);

            if (e.CombatUnit.Unit.IsDead)
            {
                return;
            }

            var unitGameObject = GetUnitGameObject(e.CombatUnit);

            var font = _uiContentStorage.GetMainFont();
            var position = unitGameObject.Position;

            var damageIndicator = new HitPointsChangedComponent(-e.Amount, e.Direction, position, font);

            unitGameObject.AddChild(damageIndicator);
        }

        private void CombatUnit_Healed(object? sender, UnitHitPointsChangedEventArgs e)
        {
            Debug.Assert(e.CombatUnit is not null);
            var unitGameObject = GetUnitGameObject(e.CombatUnit);

            var font = _uiContentStorage.GetMainFont();
            var position = unitGameObject.Position;

            var damageIndicator = new HitPointsChangedComponent(e.Amount, e.Direction, position, font);

            unitGameObject.AddChild(damageIndicator);
        }

        private void DrawBackgroundLayers(SpriteBatch spriteBatch, IReadOnlyList<Texture2D> backgrounds,
            int backgroundStartOffset,
            int backgroundMaxOffset)
        {
            for (var i = 0; i < BACKGROUND_LAYERS_COUNT; i++)
            {
                var xFloat = backgroundStartOffset + _bgCenterOffsetPercentage * (BACKGROUND_LAYERS_COUNT - i - 1) *
                    BACKGROUND_LAYERS_SPEED * backgroundMaxOffset;
                var roundedX = (int)Math.Round(xFloat);

                var position = new Vector2(roundedX, 0);
                var position3d = new Vector3(position, 0);

                var worldTransformationMatrix = _camera.GetViewTransformationMatrix();
                worldTransformationMatrix.Decompose(out var scaleVector, out _, out var translationVector);

                var shakeVector = _screenShaker.GetOffset().GetValueOrDefault(Vector2.Zero);
                var shakeVector3d = new Vector3(shakeVector, 0);

                var matrix = Matrix.CreateTranslation(translationVector + position3d + shakeVector3d)
                             * Matrix.CreateScale(scaleVector);

                spriteBatch.Begin(
                    sortMode: SpriteSortMode.Deferred,
                    blendState: BlendState.AlphaBlend,
                    samplerState: SamplerState.PointClamp,
                    depthStencilState: DepthStencilState.None,
                    rasterizerState: RasterizerState.CullNone,
                    transformMatrix: matrix);

                spriteBatch.Draw(backgrounds[i], Vector2.Zero, Color.White);

                if (i == 0 /*Cloud layer*/)
                {
                    foreach (var obj in _cloudLayerObjects)
                    {
                        obj.Draw(spriteBatch);
                    }
                }

                spriteBatch.End();
            }
        }

        private void DrawBullets(SpriteBatch spriteBatch)
        {
            foreach (var bullet in _bulletObjects)
            {
                bullet.Draw(spriteBatch);
            }
        }

        private void DrawCombatSequenceProgress(SpriteBatch spriteBatch)
        {
            if (_globeNode.CombatSequence is not null)
            {
                var combatCountRemains = _globeNode.CombatSequence.Combats.Count;

                spriteBatch.DrawString(_uiContentStorage.GetMainFont(), $"Combats remains: {combatCountRemains}",
                    new Vector2(_resolutionIndependentRenderer.VirtualBounds.Center.X, 5), Color.White);
            }
        }

        private void DrawForegroundLayers(SpriteBatch spriteBatch, Texture2D[] backgrounds, int backgroundStartOffset,
            int backgroundMaxOffset)
        {
            var xFloat = backgroundStartOffset +
                         -1 * _bgCenterOffsetPercentage * BACKGROUND_LAYERS_SPEED * 2 * backgroundMaxOffset;
            var roundedX = (int)Math.Round(xFloat);

            var position = new Vector2(roundedX, 0);
            var position3d = new Vector3(position, 0);

            var shakeVector = _screenShaker.GetOffset().GetValueOrDefault(Vector2.Zero);
            var shakeVector3d = new Vector3(shakeVector, 0);

            var worldTransformationMatrix = _camera.GetViewTransformationMatrix();
            worldTransformationMatrix.Decompose(out var scaleVector, out var _, out var translationVector);

            var matrix = Matrix.CreateTranslation(translationVector + position3d + shakeVector3d)
                         * Matrix.CreateScale(scaleVector);

            spriteBatch.Begin(
                sortMode: SpriteSortMode.Deferred,
                blendState: BlendState.AlphaBlend,
                samplerState: SamplerState.PointClamp,
                depthStencilState: DepthStencilState.None,
                rasterizerState: RasterizerState.CullNone,
                transformMatrix: matrix);

            spriteBatch.Draw(backgrounds[3], Vector2.Zero, Color.White);

            foreach (var obj in _foregroundLayerObjects)
            {
                obj.Draw(spriteBatch);
            }

            spriteBatch.End();
        }

        private void DrawGameObjects(SpriteBatch spriteBatch)
        {
            var backgroundType = BackgroundHelper.GetBackgroundType(_globeNode.Sid);

            var backgrounds = _gameObjectContentStorage.GetCombatBackgrounds(backgroundType);

            const int BG_START_OFFSET = -100;
            const int BG_MAX_OFFSET = 200;

            DrawBackgroundLayers(spriteBatch, backgrounds, BG_START_OFFSET, BG_MAX_OFFSET);

            var shakeVector = _screenShaker.GetOffset().GetValueOrDefault(Vector2.Zero);
            var shakeVector3d = new Vector3(shakeVector, 0);

            var worldTransformationMatrix = _camera.GetViewTransformationMatrix();
            worldTransformationMatrix.Decompose(out var scaleVector, out var _, out var translationVector);

            var matrix = Matrix.CreateTranslation(translationVector + shakeVector3d)
                         * Matrix.CreateScale(scaleVector);

            spriteBatch.Begin(sortMode: SpriteSortMode.Deferred,
                blendState: BlendState.AlphaBlend,
                samplerState: SamplerState.PointClamp,
                depthStencilState: DepthStencilState.None,
                rasterizerState: RasterizerState.CullNone,
                transformMatrix: matrix);

            DrawBullets(spriteBatch);

            DrawUnits(spriteBatch);

            foreach (var bullet in _bulletObjects)
            {
                bullet.Draw(spriteBatch);
            }

            spriteBatch.End();

            DrawForegroundLayers(spriteBatch, backgrounds, BG_START_OFFSET, BG_MAX_OFFSET);
        }

        private void DrawHud(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(sortMode: SpriteSortMode.Deferred,
                blendState: BlendState.AlphaBlend,
                samplerState: SamplerState.PointClamp,
                depthStencilState: DepthStencilState.None,
                rasterizerState: RasterizerState.CullNone,
                transformMatrix: _camera.GetViewTransformationMatrix());

            if (_сombat.CurrentUnit?.Unit.IsPlayerControlled == true && !_animationManager.HasBlockers)
            {
                if (_combatSkillsPanel is not null)
                {
                    _combatSkillsPanel.Draw(spriteBatch);
                }

                foreach (var button in _hudButtons)
                {
                    button.Draw(spriteBatch);
                }
            }

            try
            {
                _unitPanelController?.Draw(spriteBatch);

                DrawCombatSequenceProgress(spriteBatch);
            }
            catch
            {
                // TODO Fix NRE in the end of the combat with more professional way 
            }

            spriteBatch.End();
        }

        private void DrawUnits(SpriteBatch spriteBatch)
        {
            var corpseList = _corpseObjects.OrderBy(x => x.GetZIndex()).ToArray();
            foreach (var gameObject in corpseList)
            {
                gameObject.Draw(spriteBatch);
            }

            var list = _gameObjects.OrderBy(x => x.GetZIndex()).ToArray();
            foreach (var gameObject in list)
            {
                gameObject.Draw(spriteBatch);
            }
        }

        private void GainEquipmentItems(GlobeNode globeNode, Player? player)
        {
            var equipmentItemType = globeNode.EquipmentItem;

            var targetUnit = GetUnitByEquipmentOrNull(player: player, equipmentItemType: equipmentItemType);

            targetUnit?.GainEquipmentItem(1);
        }

        private Unit? GetUnitByEquipmentOrNull(Player? player, EquipmentItemType? equipmentItemType)
        {
            var targetUnitScheme =
                UnsortedHelpers.GetPlayerPersonSchemeByEquipmentType(_unitSchemeCatalog, equipmentItemType);
            var targetUnit = player.GetAll().SingleOrDefault(x => x.UnitScheme == targetUnitScheme);
            return targetUnit;
        }

        private UnitGameObject GetUnitGameObject(CombatUnit combatUnit)
        {
            return _gameObjects.First(x => x.CombatUnit == combatUnit);
        }

        private Vector2 GetUnitPosition(int index, bool isPlayerControlled)
        {
            var predefinedPosition = _unitPredefinedPositions[index];

            Vector2 calculatedPosition;

            if (isPlayerControlled)
            {
                calculatedPosition = predefinedPosition;
            }
            else
            {
                var width = _resolutionIndependentRenderer.VirtualWidth;
                // Move from right edge.
                var xMirror = width - predefinedPosition.X;
                calculatedPosition = new Vector2(xMirror, predefinedPosition.Y);
            }

            return calculatedPosition;
        }

        private void HandleBackgrounds()
        {
            var mouse = Mouse.GetState();
            var mouseRir = _resolutionIndependentRenderer.ScaleMouseToScreenCoordinates(new Vector2(mouse.X, mouse.Y));
            var screenCenterX = _resolutionIndependentRenderer.VirtualBounds.Center.X;
            var rawPercentage = (mouseRir.X - screenCenterX) / screenCenterX;
            _bgCenterOffsetPercentage = NormalizePercentage(rawPercentage);
        }

        private void HandleBullets(GameTime gameTime)
        {
            foreach (var bullet in _bulletObjects.ToArray())
            {
                if (bullet.IsDestroyed)
                {
                    _bulletObjects.Remove(bullet);
                }
                else
                {
                    bullet.Update(gameTime);
                }
            }
        }

        private void HandleCombatHud()
        {
            if (!_interactButtonClicked)
            {
                var skillButtonFixedList = _hudButtons.ToArray();
                foreach (var button in skillButtonFixedList)
                {
                    button.Update(_resolutionIndependentRenderer);
                }

                _combatSkillsPanel?.Update(_resolutionIndependentRenderer);
            }
        }

        private IReadOnlyCollection<XpAward> HandleGainXp(ICollection<CombatSource> completedCombats)
        {
            var combatSequenceCoeffs = new[] { 1f, 0 /*not used*/, 1.25f, /*not used*/0, 1.5f };

            var aliveUnits = _сombat.Units.Where(x => x.Unit.IsPlayerControlled && !x.Unit.IsDead).ToArray();
            var monsters = completedCombats.SelectMany(x => x.EnemyGroup.GetUnits()).ToArray();

            var sequenceBonus = combatSequenceCoeffs[completedCombats.Count - 1];
            var summaryXp = (int)Math.Round(monsters.Sum(x => x.XpReward) * sequenceBonus);
            var xpPerPlayerUnit = summaryXp / aliveUnits.Length;

            var remains = summaryXp - (xpPerPlayerUnit * aliveUnits.Length);

            var remainsUsed = false;
            var list = new List<XpAward>();
            foreach (var unit in aliveUnits)
            {
                var gainedXp = xpPerPlayerUnit;

                if (!remainsUsed)
                {
                    gainedXp += remains;
                    remainsUsed = true;
                }

                var item = new XpAward
                {
                    StartXp = unit.Unit.Xp,
                    Unit = unit.Unit,
                    XpAmount = gainedXp,
                    XpToLevelupSelector = () => unit.Unit.LevelUpXp
                };
                list.Add(item);
            }

            return list;
        }

        private void HandleGlobe(CombatResult result)
        {
            _bossWasDefeat = false;
            _finalBossWasDefeat = false;

            switch (result)
            {
                case CombatResult.Victory:
                    if (!_сombat.CombatSource.IsTrainingOnly)
                    {
                        _сombat.Biome.Level++;
                    }

                    var nodeIndex = _globeNode.Index;
                    var unlockedBiomeIndex = nodeIndex + 1;

                    var unlockedNode = _globe.CurrentBiome.Nodes.SingleOrDefault(x => x.Index == unlockedBiomeIndex);
                    if (unlockedNode is not null)
                    {
                        unlockedNode.IsAvailable = true;
                    }

                    if (_globe.CurrentEvent is not null)
                    {
                        _globe.CurrentEvent.Completed = true;

                        _globe.CurrentEventNode = _globe.CurrentEvent.AfterCombatStartNode;
                    }

                    if (_сombat.CombatSource.IsBossLevel)
                    {
                        _сombat.Biome.IsComplete = true;
                        _bossWasDefeat = true;

                        if (_сombat.Biome.IsFinal)
                        {
                            _finalBossWasDefeat = true;
                        }
                    }

                    break;

                case CombatResult.Defeat:
                    var levelDiff = _сombat.Biome.Level - _сombat.Biome.MinLevel;
                    _сombat.Biome.Level = Math.Max(levelDiff / 2, _сombat.Biome.MinLevel);

                    break;

                default:
                    throw new InvalidOperationException("Unknown combat result.");
            }
        }

        private void HandleUnits(GameTime gameTime)
        {
            foreach (var gameObject in _gameObjects.ToArray())
            {
                gameObject.Update(gameTime);
            }

            foreach (var gameObject in _corpseObjects.ToArray())
            {
                gameObject.Update(gameTime);
            }
        }

        private void InitHudButton(UnitGameObject target, CombatSkillCard skillCard)
        {
            var interactButton = new UnitButton(
                _uiContentStorage.GetButtonTexture(),
                new Rectangle((target.Position - new Vector2(64, 128)).ToPoint(),
                    new Point(128, 128)),
                _gameObjectContentStorage);

            interactButton.OnClick += (s, e) =>
            {
                if (_interactButtonClicked)
                {
                    return;
                }

                _hudButtons.Clear();
                _interactButtonClicked = true;
                _сombat.UseSkill(skillCard.Skill, target.CombatUnit);
            };

            _hudButtons.Add(interactButton);
        }

        private static float NormalizePercentage(float value)
        {
            return value switch
            {
                < -1 => -1,
                > 1 => 1,
                _ => value
            };
        }

        private void RefreshHudButtons(CombatSkillCard? skillCard)
        {
            _interactButtonClicked = false;
            _hudButtons.Clear();

            if (skillCard is null)
            {
                return;
            }

            if (_сombat.CurrentUnit is null)
            {
                Debug.Fail("WTF!");
                return;
            }

            var availableTargetGameObjects = _gameObjects.Where(x => !x.CombatUnit.Unit.IsDead);
            foreach (var target in availableTargetGameObjects)
            {
                if (skillCard.Skill.TargetType == SkillTargetType.Self)
                {
                    if (target.CombatUnit.Unit != _сombat.CurrentUnit.Unit)
                    {
                        continue;
                    }

                    InitHudButton(target, skillCard);
                }
                else if (skillCard.Skill.TargetType == SkillTargetType.Enemy)
                {
                    if (skillCard.Skill.Type == SkillType.Melee)
                    {
                        var isTargetInTankPosition = target.CombatUnit.Index == 0;
                        if (isTargetInTankPosition)
                        {
                            if (skillCard.Skill.TargetType == SkillTargetType.Enemy
                                && target.CombatUnit.Unit.IsPlayerControlled ==
                                _сombat.CurrentUnit.Unit.IsPlayerControlled)
                            {
                                continue;
                            }

                            InitHudButton(target, skillCard);
                        }
                        else
                        {
                            var isAnyUnitsInTaskPosition = _gameObjects.Where(x =>
                                    !x.CombatUnit.Unit.IsDead && !x.CombatUnit.Unit.IsPlayerControlled &&
                                    x.CombatUnit.Index == 0)
                                .Any();

                            if (!isAnyUnitsInTaskPosition)
                            {
                                if (skillCard.Skill.TargetType == SkillTargetType.Enemy
                                    && target.CombatUnit.Unit.IsPlayerControlled ==
                                    _сombat.CurrentUnit.Unit.IsPlayerControlled)
                                {
                                    continue;
                                }

                                InitHudButton(target, skillCard);
                            }
                        }
                    }
                    else
                    {
                        if (skillCard.Skill.TargetType == SkillTargetType.Enemy
                            && target.CombatUnit.Unit.IsPlayerControlled == _сombat.CurrentUnit.Unit.IsPlayerControlled)
                        {
                            continue;
                        }

                        InitHudButton(target, skillCard);
                    }
                }
                else
                {
                    if (skillCard.Skill.TargetType == SkillTargetType.Friendly
                        && target.CombatUnit.Unit.IsPlayerControlled != _сombat.CurrentUnit.Unit.IsPlayerControlled)
                    {
                        continue;
                    }

                    InitHudButton(target, skillCard);
                }
            }
        }

        private void RestoreGroupAfterCombat()
        {
            foreach (var unit in _globe.Player.GetAll())
            {
                unit.RestoreHitPointsAfterCombat();
                unit.RestoreManaPoint();
            }
        }

        private void ShowCombatResultModal(bool isVictory)
        {
            CombatResultModal combatResultModal;

            if (isVictory)
            {
                var completedCombats = _globeNode.CombatSequence.CompletedCombats;
                completedCombats.Add(_сombat.CombatSource);

                var currentCombatList = _сombat.Node.CombatSequence.Combats.ToList();
                if (currentCombatList.Count == 1)
                {
                    var xpItems = HandleGainXp(completedCombats).ToArray();
                    ApplyXp(xpItems);
                    GainEquipmentItems(_globeNode, _globeProvider.Globe.Player);
                    HandleGlobe(CombatResult.Victory);

                    var soundtrackManager = Game.Services.GetService<SoundtrackManager>();
                    soundtrackManager.PlayVictoryTrack();

                    combatResultModal = new CombatResultModal(_uiContentStorage, _resolutionIndependentRenderer,
                        CombatResult.Victory,
                        xpItems,
                        _сombat.CombatSource);
                }
                else
                {
                    combatResultModal = new CombatResultModal(_uiContentStorage, _resolutionIndependentRenderer,
                        CombatResult.NextCombat,
                        Array.Empty<XpAward>(),
                        _сombat.CombatSource);
                }
            }
            else
            {
                var soundtrackManager = Game.Services.GetService<SoundtrackManager>();
                soundtrackManager.PlayDefeatTrack();

                HandleGlobe(CombatResult.Defeat);

                combatResultModal = new CombatResultModal(_uiContentStorage, _resolutionIndependentRenderer,
                    CombatResult.Defeat,
                    Array.Empty<XpAward>(),
                    _сombat.CombatSource);
            }

            AddModal(combatResultModal, isLate: false);

            combatResultModal.Closed += CombatResultModal_Closed;
        }

        private void UpdateBackgroundObjects(GameTime gameTime)
        {
            foreach (var obj in _foregroundLayerObjects)
            {
                obj.Update(gameTime);
            }

            foreach (var obj in _cloudLayerObjects)
            {
                obj.Update(gameTime);
            }
        }

        private void UpdateCombatFinished(GameTime gameTime)
        {
            _combatFinishedCounter += gameTime.ElapsedGameTime.TotalSeconds;

            if (_combatFinishedCounter >= 2 && !_combatResultModalShown)
            {
                _combatResultModalShown = true;
                ShowCombatResultModal(_combatFinishedVictory.Value);
            }
        }
    }
}