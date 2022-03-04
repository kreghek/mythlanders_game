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

        private readonly AnimationManager _animationManager;
        private readonly IList<IInteractionDelivery> _bulletObjects;
        private readonly Camera2D _camera;
        private readonly IReadOnlyCollection<IBackgroundObject> _cloudLayerObjects;
        private readonly Core.Combat _combat;
        private readonly IList<CorpseGameObject> _corpseObjects;
        private readonly IDice _dice;
        private readonly IEventCatalog _eventCatalog;
        private readonly IReadOnlyList<IBackgroundObject> _foregroundLayerObjects;
        private readonly GameObjectContentStorage _gameObjectContentStorage;
        private readonly IList<UnitGameObject> _gameObjects;
        private readonly Globe _globe;
        private readonly GlobeNode _globeNode;
        private readonly GlobeProvider _globeProvider;
        private readonly IList<ButtonBase> _interactionButtons;
        private readonly ScreenShaker _screenShaker;
        private readonly GameSettings _settings;
        private readonly IUiContentStorage _uiContentStorage;

        private readonly Vector2[] _unitPredefinedPositions;
        private readonly IUnitSchemeCatalog _unitSchemeCatalog;

        private float _bgCenterOffsetPercentage;
        private bool _bossWasDefeat;
        private double _combatFinishedCounter;

        private bool? _combatFinishedVictory;

        private bool _combatInitialized;
        private bool _combatResultModalShown;
        private CombatSkillPanel? _combatSkillsPanel;

        private bool _finalBossWasDefeat;


        private bool _interactButtonClicked;
        private UnitStatePanelController? _unitStatePanelController;

        public CombatScreen(EwarGame game) : base(game)
        {
            var soundtrackManager = Game.Services.GetService<SoundtrackManager>();

            _globeProvider = game.Services.GetService<GlobeProvider>();
            _camera = Game.Services.GetService<Camera2D>();

            _globe = _globeProvider.Globe;

            _combat = _globe.ActiveCombat ??
                      throw new InvalidOperationException(
                          nameof(_globe.ActiveCombat) + " can't be null in this screen.");

            _globeNode = _combat.Node;
            soundtrackManager.PlayBattleTrack(_globe.CurrentBiome.Type);

            _gameObjects = new List<UnitGameObject>();
            _corpseObjects = new List<CorpseGameObject>();
            _bulletObjects = new List<IInteractionDelivery>();
            _interactionButtons = new List<ButtonBase>();

            _gameObjectContentStorage = game.Services.GetService<GameObjectContentStorage>();
            _uiContentStorage = game.Services.GetService<IUiContentStorage>();
            _animationManager = game.Services.GetService<AnimationManager>();
            _dice = Game.Services.GetService<IDice>();

            var bgofSelector = Game.Services.GetService<BackgroundObjectFactorySelector>();

            _unitSchemeCatalog = game.Services.GetService<IUnitSchemeCatalog>();
            _eventCatalog = game.Services.GetService<IEventCatalog>();

            var backgroundObjectFactory = bgofSelector.GetBackgroundObjectFactory(_globeNode.Sid);

            _cloudLayerObjects = backgroundObjectFactory.CreateCloudLayerObjects();
            _foregroundLayerObjects = backgroundObjectFactory.CreateForegroundLayerObjects();

            _settings = game.Services.GetService<GameSettings>();

            _unitPredefinedPositions = new[]
            {
                new Vector2(335, 300),
                new Vector2(305, 250),
                new Vector2(305, 350),
                new Vector2(215, 250),
                new Vector2(215, 350),
                new Vector2(165, 300)
            };

            _screenShaker = new ScreenShaker();
        }

        protected override IList<ButtonBase> CreateMenu()
        {
            var surrenderButton = new ResourceTextButton(nameof(UiResource.SurrenderButtonTitle),
                _uiContentStorage.GetButtonTexture(), _uiContentStorage.GetMainFont());
            surrenderButton.OnClick += EscapeButton_OnClick;

            return new ButtonBase[] { surrenderButton };
        }

        protected override void DrawContentWithoutMenu(SpriteBatch spriteBatch, Rectangle contentRectangle)
        {
            ResolutionIndependentRenderer.BeginDraw();

            DrawGameObjects(spriteBatch);

            DrawHud(spriteBatch, contentRectangle);
        }

        protected override void UpdateContent(GameTime gameTime)
        {
            base.UpdateContent(gameTime);

            if (!_globe.Player.HasAbility(PlayerAbility.ReadCombatTutorial) &&
                !_globe.Player.HasAbility(PlayerAbility.SkipTutorials))
            {
                _globe.Player.AddPlayerAbility(PlayerAbility.ReadCombatTutorial);
                var tutorialModal = new TutorialModal(new CombatTutorialPageDrawer(_uiContentStorage),
                    _uiContentStorage, ResolutionIndependentRenderer, _globe.Player);
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

                if (!_combat.Finished && _combatFinishedVictory is null)
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

            _combat.Update();
        }

        private static void AddMonstersFromCombatIntoKnownMonsters(Unit monster,
            ICollection<UnitScheme> playerKnownMonsters)
        {
            var scheme = monster.UnitScheme;
            if (playerKnownMonsters.All(x => x != scheme))
            {
                playerKnownMonsters.Add(scheme);
            }
        }

        private static void ApplyCombatReward(IReadOnlyCollection<CombatRewardsItem> xpItems, Player player)
        {
            foreach (var item in xpItems)
            {
                var inventoryItem = player.Inventory.Single(x => x.Type == item.Xp.Type);

                inventoryItem.Amount += item.Xp.Amount;
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
            _interactionButtons.Clear();
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
            _combatSkillsPanel.SelectedSkill = null;
            _combatSkillsPanel.IsEnabled = false;
        }

        private void Combat_UnitDied(object? sender, CombatUnit e)
        {
            e.UnsubscribeHandlers();

            var unitGameObject = GetUnitGameObject(e);

            var corpse = unitGameObject.CreateCorpse();
            _corpseObjects.Add(corpse);
        }

        private void Combat_UnitEntered(object? sender, CombatUnit combatUnit)
        {
            if (combatUnit.Unit.UnitScheme.IsMonster)
            {
                AddMonstersFromCombatIntoKnownMonsters(combatUnit.Unit, _globe.Player.KnownMonsters);
            }

            var position = GetUnitPosition(combatUnit.Index, combatUnit.Unit.IsPlayerControlled);
            var gameObject =
                new UnitGameObject(combatUnit, position, _gameObjectContentStorage, _camera, _screenShaker,
                    _animationManager);
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
            var font = _uiContentStorage.GetCombatIndicatorFont();

            var passIndicator = new SkipTextIndicator(textPosition, font);

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

            if (e.Unit.IsDead)
            {
                return;
            }

            var selectedUnit = e;

            _combatSkillsPanel.IsEnabled = true;
            _combatSkillsPanel.Unit = selectedUnit;
            _combatSkillsPanel.SelectedSkill = selectedUnit.CombatCards.First();
            var unitGameObject = GetUnitGameObject(e);
            unitGameObject.IsActive = true;
        }

        private void CombatInitialize()
        {
            _combatSkillsPanel = new CombatSkillPanel(_uiContentStorage.GetButtonTexture(), _uiContentStorage);
            _combatSkillsPanel.SkillSelected += CombatSkillsPanel_CardSelected;
            _combat.ActiveCombatUnitChanged += Combat_UnitChanged;
            _combat.CombatUnitIsReadyToControl += Combat_UnitReadyToControl;
            _combat.CombatUnitEntered += Combat_UnitEntered;
            _combat.CombatUnitRemoved += Combat_CombatUnitRemoved;
            _combat.UnitDied += Combat_UnitDied;
            _combat.ActionGenerated += Combat_ActionGenerated;
            _combat.Finish += Combat_Finish;
            _combat.UnitHasBeenDamaged += Combat_UnitHasBeenDamaged;
            _combat.UnitPassedTurn += Combat_UnitPassed;
            _combat.Initialize();
            _combat.Update();

            var settigs = Game.Services.GetService<GameSettings>();
            // TODO Remove then effects would be developed.
            _unitStatePanelController = new UnitStatePanelController(_combat,
                _uiContentStorage, _gameObjectContentStorage, settigs.Mode == GameMode.Full);
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
                currentCombatList.Remove(_combat.CombatSource);
                _globeNode.CombatSequence.Combats = currentCombatList;

                if (_combat.Node.CombatSequence.Combats.Any())
                {
                    var nextCombat = _globeNode.CombatSequence.Combats.First();
                    _globe.ActiveCombat = new Core.Combat(_globe.Player.Party,
                        _globeNode,
                        nextCombat,
                        _combat.Biome,
                        _dice,
                        _combat.IsAutoplay);

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

                            if (_settings.Mode == GameMode.Full)
                            {
                                _globeProvider.StoreCurrentGlobe();
                            }
                        }
                        else
                        {
                            _globeProvider.Globe.UpdateNodes(_dice, _unitSchemeCatalog, _eventCatalog);
                            _globeProvider.Globe.CurrentBiome =
                                _globe.Biomes.Single(x => x.Type == _combat.Biome.UnlockBiome);
                            var startGlobeNode = _globeProvider.Globe.CurrentBiome.Nodes.Single(x => x.IsAvailable);
                            _globe.CurrentEvent = startGlobeNode.AssignedEvent;
                            _globe.CurrentEventNode = _globe.CurrentEvent.BeforeCombatStartNode;

                            _globe.CurrentEvent.Counter++;

                            var combatSource = startGlobeNode.CombatSequence.Combats.First();
                            _globe.ActiveCombat = new Core.Combat(_globe.Player.Party, startGlobeNode,
                                combatSource, _globeProvider.Globe.CurrentBiome, _dice, isAutoplay: false);

                            ScreenManager.ExecuteTransition(this, ScreenTransition.Event);
                        }
                    }
                    else
                    {
                        if (_globe.CurrentEventNode is null)
                        {
                            _globeProvider.Globe.UpdateNodes(_dice, _unitSchemeCatalog, _eventCatalog);
                            ScreenManager.ExecuteTransition(this, ScreenTransition.Biome);

                            if (_settings.Mode == GameMode.Full)
                            {
                                _globeProvider.StoreCurrentGlobe();
                            }
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

        private void CombatSkillsPanel_CardSelected(object? sender, CombatSkill? skillCard)
        {
            RefreshHudButtons(skillCard);
        }

        private void CombatUnit_HasAvoidedDamage(object? sender, EventArgs e)
        {
            Debug.Assert(sender is not null);
            var combatUnit = (CombatUnit)sender;
            var unitGameObject = GetUnitGameObject(combatUnit);
            var textPosition = GetUnitGameObject(combatUnit).Position;
            var font = _uiContentStorage.GetCombatIndicatorFont();

            var passIndicator = new EvasionTextIndicator(textPosition, font);

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

            var font = _uiContentStorage.GetCombatIndicatorFont();
            var position = unitGameObject.Position;

            var nextIndex = GetIndicatorNextIndex(unitGameObject);

            var damageIndicator =
                new HitPointsChangedTextIndicator(-e.Amount, e.Direction, position, font, nextIndex ?? 0);

            unitGameObject.AddChild(damageIndicator);
        }

        private void CombatUnit_Healed(object? sender, UnitHitPointsChangedEventArgs e)
        {
            Debug.Assert(e.CombatUnit is not null);
            var unitGameObject = GetUnitGameObject(e.CombatUnit);

            var font = _uiContentStorage.GetCombatIndicatorFont();
            var position = unitGameObject.Position;

            var nextIndex = GetIndicatorNextIndex(unitGameObject);

            var damageIndicator =
                new HitPointsChangedTextIndicator(e.Amount, e.Direction, position, font, nextIndex ?? 0);

            unitGameObject.AddChild(damageIndicator);
        }

        private static CombatRewardsItem CreateReward(IReadOnlyCollection<ResourceItem> inventory,
            EquipmentItemType resourceType, int amount)
        {
            var inventoryItem = inventory.Single(x => x.Type == resourceType);
            var item = new CombatRewardsItem
            {
                Xp = new CountableRewardStat
                {
                    StartValue = inventoryItem.Amount,
                    Amount = amount,
                    Type = resourceType
                }
            };

            return item;
        }

        private static CombatRewardsItem CreateXpReward(IReadOnlyCollection<ResourceItem> inventory, int amount)
        {
            const EquipmentItemType EXPIRIENCE_POINTS_TYPE = EquipmentItemType.ExpiriencePoints;
            return CreateReward(inventory, EXPIRIENCE_POINTS_TYPE, amount);
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
                var sumSequenceLength = _globeNode.CombatSequence.Combats.Count +
                                        _globeNode.CombatSequence.CompletedCombats.Count;

                var completeCombatCount = _globeNode.CombatSequence.CompletedCombats.Count + 1;

                var position = new Vector2(ResolutionIndependentRenderer.VirtualBounds.Center.X, 5);

                spriteBatch.DrawString(_uiContentStorage.GetMainFont(),
                    string.Format(UiResource.CombatProgressTemplate, completeCombatCount, sumSequenceLength),
                    position, Color.White);

                spriteBatch.DrawString(_uiContentStorage.GetMainFont(),
                    string.Format(UiResource.MonsterDangerTemplate, _combat.CombatSource.Level),
                    position + new Vector2(0, 10), Color.White);
            }
        }

        private void DrawCombatSkillsPanel(SpriteBatch spriteBatch, Rectangle contentRectangle)
        {
            if (_combatSkillsPanel is not null)
            {
                const int COMBAT_SKILLS_PANEL_WIDTH = 480;
                const int COMBAT_SKILLS_PANEL_HEIGHT = 64;
                _combatSkillsPanel.Rect = new Rectangle(
                    contentRectangle.Center.X - COMBAT_SKILLS_PANEL_WIDTH / 2,
                    contentRectangle.Bottom - COMBAT_SKILLS_PANEL_HEIGHT,
                    COMBAT_SKILLS_PANEL_WIDTH, COMBAT_SKILLS_PANEL_HEIGHT);
                _combatSkillsPanel.Draw(spriteBatch);
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

        private void DrawHud(SpriteBatch spriteBatch, Rectangle contentRectangle)
        {
            spriteBatch.Begin(sortMode: SpriteSortMode.Deferred,
                blendState: BlendState.AlphaBlend,
                samplerState: SamplerState.PointClamp,
                depthStencilState: DepthStencilState.None,
                rasterizerState: RasterizerState.CullNone,
                transformMatrix: _camera.GetViewTransformationMatrix());

            if (_combat.CurrentUnit?.Unit.IsPlayerControlled == true && !_animationManager.HasBlockers)
            {
                DrawCombatSkillsPanel(spriteBatch, contentRectangle);
                DrawInteractionButtons(spriteBatch);
            }

            try
            {
                DrawUnitStatePanels(spriteBatch, contentRectangle);
                DrawCombatSequenceProgress(spriteBatch);
            }
            catch
            {
                // TODO Fix NRE in the end of the combat with more professional way 
            }

            spriteBatch.End();
        }

        private void DrawInteractionButtons(SpriteBatch spriteBatch)
        {
            foreach (var button in _interactionButtons)
            {
                button.Draw(spriteBatch);
            }
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

        private void DrawUnitStatePanels(SpriteBatch spriteBatch, Rectangle contentRectangle)
        {
            _unitStatePanelController?.Draw(spriteBatch, contentRectangle);
        }

        private void EscapeButton_OnClick(object? sender, EventArgs e)
        {
            _combat.Surrender();
            _combatFinishedVictory = false;
        }

        private static int? GetIndicatorNextIndex(UnitGameObject? unitGameObject)
        {
            var currentIndex = unitGameObject.GetCurrentIndicatorIndex();
            var nextIndex = currentIndex + 1;
            return nextIndex;
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
                var width = ResolutionIndependentRenderer.VirtualWidth;
                // Move from right edge.
                var xMirror = width - predefinedPosition.X;
                calculatedPosition = new Vector2(xMirror, predefinedPosition.Y);
            }

            return calculatedPosition;
        }

        private void HandleBackgrounds()
        {
            var mouse = Mouse.GetState();
            var mouseRir = ResolutionIndependentRenderer.ScaleMouseToScreenCoordinates(new Vector2(mouse.X, mouse.Y));
            var screenCenterX = ResolutionIndependentRenderer.VirtualBounds.Center.X;
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
                var skillButtonFixedList = _interactionButtons.ToArray();
                foreach (var button in skillButtonFixedList)
                {
                    button.Update(ResolutionIndependentRenderer);
                }

                _combatSkillsPanel?.Update(ResolutionIndependentRenderer);
            }
        }

        private void HandleGlobe(CombatResult result)
        {
            _bossWasDefeat = false;
            _finalBossWasDefeat = false;

            switch (result)
            {
                case CombatResult.Victory:
                    if (!_combat.CombatSource.IsTrainingOnly)
                    {
                        _combat.Biome.Level++;
                    }

                    var nodeIndex = (int)_globeNode.Sid;
                    var unlockedLocationIndex = nodeIndex + 1;
                    var unlockedLocationSid = (GlobeNodeSid)unlockedLocationIndex;

                    var unlockedNode =
                        _globe.CurrentBiome.Nodes.SingleOrDefault(x => x.Sid == _globeNode.UnlockNodeSid);
                    if (unlockedNode is not null)
                    {
                        unlockedNode.IsAvailable = true;
                    }

                    if (_globe.CurrentEvent is not null)
                    {
                        _globe.CurrentEvent.Completed = true;

                        _globe.CurrentEventNode = _globe.CurrentEvent.AfterCombatStartNode;
                    }

                    if (_combat.CombatSource.IsBossLevel)
                    {
                        _combat.Biome.IsComplete = true;
                        _bossWasDefeat = true;
                        // Then the player defeat first boss he can split characters on a tanks and dd+support line.
                        _globeProvider.Globe.Player.AddPlayerAbility(PlayerAbility.AvailableTanks);

                        if (_combat.Biome.IsFinal)
                        {
                            _finalBossWasDefeat = true;
                        }
                    }

                    break;

                case CombatResult.Defeat:
                    var levelDiff = _combat.Biome.Level - _combat.Biome.MinLevel;
                    _combat.Biome.Level = Math.Max(levelDiff / 2, _combat.Biome.MinLevel);

                    break;

                default:
                    throw new InvalidOperationException("Unknown combat result.");
            }
        }

        private CombatRewards HandleRewardGaining(
            ICollection<CombatSource> completedCombats,
            GlobeNode globeNode,
            Player? player)
        {
            var combatSequenceXpBonuses = UnsortedHelpers.GetCombatSequenceXpBonuses();

            var monsters = completedCombats.SelectMany(x => x.EnemyGroup.GetUnits()).ToArray();

            var sequenceBonus = combatSequenceXpBonuses[completedCombats.Count - 1];
            var summaryXp = (int)Math.Round(monsters.Sum(x => x.XpReward) * sequenceBonus);

            var rewardList = new List<CombatRewardsItem>();
            if (globeNode.EquipmentItem is not null)
            {
                var rewardItem = CreateReward(player.Inventory, globeNode.EquipmentItem.Value, amount: 1);
                rewardList.Add(rewardItem);
            }

            var gainedXp = summaryXp;
            var xpRewardItem = CreateXpReward(player.Inventory, gainedXp);
            rewardList.Add(xpRewardItem);

            var combatRewards = new CombatRewards
            {
                BiomeProgress = new ProgressionRewardStat
                {
                    StartValue = _combat.Biome.Level,
                    Amount = 1,
                    ValueToLevelupSelector = () => _combat.Biome.MinLevel + 15
                },
                InventoryRewards = rewardList
            };

            return combatRewards;
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

        private void InitHudButton(UnitGameObject target, CombatSkill skillCard)
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

                _interactionButtons.Clear();
                _interactButtonClicked = true;
                _combat.UseSkill(skillCard.Skill, target.CombatUnit);
            };

            _interactionButtons.Add(interactButton);
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

        private void RefreshHudButtons(CombatSkill? skillCard)
        {
            _interactButtonClicked = false;
            _interactionButtons.Clear();

            if (skillCard is null)
            {
                return;
            }

            if (_combat.CurrentUnit is null)
            {
                Debug.Fail("WTF!");
                return;
            }

            var availableTargetGameObjects = _gameObjects.Where(x => !x.CombatUnit.Unit.IsDead);
            foreach (var target in availableTargetGameObjects)
            {
                if (skillCard.Skill.TargetType == SkillTargetType.Self)
                {
                    if (target.CombatUnit.Unit != _combat.CurrentUnit.Unit)
                    {
                        continue;
                    }

                    InitHudButton(target, skillCard);
                }
                else if (skillCard.Skill.TargetType == SkillTargetType.Enemy)
                {
                    if (skillCard.Skill.Type == SkillType.Melee)
                    {
                        var isTargetInTankPosition = target.CombatUnit.IsInTankLine;
                        if (isTargetInTankPosition)
                        {
                            if (skillCard.Skill.TargetType == SkillTargetType.Enemy
                                && target.CombatUnit.Unit.IsPlayerControlled ==
                                _combat.CurrentUnit.Unit.IsPlayerControlled)
                            {
                                continue;
                            }

                            InitHudButton(target, skillCard);
                        }
                        else
                        {
                            var isAnyUnitsInTaskPosition = _gameObjects.Where(x =>
                                    !x.CombatUnit.Unit.IsDead && !x.CombatUnit.Unit.IsPlayerControlled &&
                                    x.CombatUnit.IsInTankLine)
                                .Any();

                            if (!isAnyUnitsInTaskPosition)
                            {
                                if (skillCard.Skill.TargetType == SkillTargetType.Enemy
                                    && target.CombatUnit.Unit.IsPlayerControlled ==
                                    _combat.CurrentUnit.Unit.IsPlayerControlled)
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
                            && target.CombatUnit.Unit.IsPlayerControlled == _combat.CurrentUnit.Unit.IsPlayerControlled)
                        {
                            continue;
                        }

                        InitHudButton(target, skillCard);
                    }
                }
                else
                {
                    if (skillCard.Skill.TargetType == SkillTargetType.Friendly
                        && target.CombatUnit.Unit.IsPlayerControlled != _combat.CurrentUnit.Unit.IsPlayerControlled)
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
                completedCombats.Add(_combat.CombatSource);

                var currentCombatList = _combat.Node.CombatSequence.Combats.ToList();
                if (currentCombatList.Count == 1)
                {
                    var xpItems = HandleRewardGaining(completedCombats, _globeNode, _globeProvider.Globe.Player);
                    ApplyCombatReward(xpItems.InventoryRewards, _globeProvider.Globe.Player);
                    HandleGlobe(CombatResult.Victory);

                    var soundtrackManager = Game.Services.GetService<SoundtrackManager>();
                    soundtrackManager.PlayVictoryTrack();

                    combatResultModal = new CombatResultModal(
                        _uiContentStorage,
                        _gameObjectContentStorage,
                        ResolutionIndependentRenderer,
                        CombatResult.Victory,
                        xpItems);
                }
                else
                {
                    combatResultModal = new CombatResultModal(
                        _uiContentStorage,
                        _gameObjectContentStorage,
                        ResolutionIndependentRenderer,
                        CombatResult.NextCombat,
                        new CombatRewards
                        {
                            BiomeProgress = new ProgressionRewardStat(),
                            InventoryRewards = Array.Empty<CombatRewardsItem>()
                        });
                }
            }
            else
            {
                var soundtrackManager = Game.Services.GetService<SoundtrackManager>();
                soundtrackManager.PlayDefeatTrack();

                HandleGlobe(CombatResult.Defeat);

                combatResultModal = new CombatResultModal(
                    _uiContentStorage,
                    _gameObjectContentStorage,
                    ResolutionIndependentRenderer,
                    CombatResult.Defeat,
                    new CombatRewards
                    {
                        BiomeProgress = new ProgressionRewardStat
                        {
                            StartValue = _combat.Biome.Level,
                            Amount = _combat.Biome.Level / 2,
                            ValueToLevelupSelector = () => 25
                        },
                        InventoryRewards = Array.Empty<CombatRewardsItem>()
                    });
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