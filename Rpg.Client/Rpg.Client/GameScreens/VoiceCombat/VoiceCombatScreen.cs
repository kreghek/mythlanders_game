using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Rpg.Client.Core;
using Rpg.Client.Core.Dialogues;
using Rpg.Client.Core.Skills;
using Rpg.Client.Engine;
using Rpg.Client.GameScreens.Combat;
using Rpg.Client.GameScreens.Combat.GameObjects;
using Rpg.Client.GameScreens.Combat.GameObjects.Background;
using Rpg.Client.GameScreens.Combat.Tutorial;
using Rpg.Client.GameScreens.Combat.Ui;
using Rpg.Client.GameScreens.Common;
using Rpg.Client.ScreenManagement;

namespace Rpg.Client.GameScreens.VoiceCombat
{
    internal class VoiceCombatScreen : GameScreenWithMenuBase
    {
        private const int BACKGROUND_LAYERS_COUNT = 3;
        private const float BACKGROUND_LAYERS_SPEED = 0.1f;

        private readonly AnimationManager _animationManager;
        private readonly IList<IInteractionDelivery> _bulletObjects;
        private readonly Camera2D _camera;
        private readonly IReadOnlyCollection<IBackgroundObject> _cloudLayerObjects;
        private readonly Core.Combat _combat;
        private readonly IList<CorpseGameObject> _corpseObjects;
        private readonly EventNode _currentEventNode;
        private readonly IDice _dice;
        private readonly IEventCatalog _eventCatalog;
        private readonly IReadOnlyList<IBackgroundObject> _foregroundLayerObjects;
        private readonly GameObjectContentStorage _gameObjectContentStorage;
        private readonly IList<UnitGameObject> _gameObjects;
        private readonly Globe _globe;
        private readonly GlobeNode _globeNode;
        private readonly GlobeProvider _globeProvider;
        private readonly IList<ButtonBase> _interactionButtons;
        private readonly ResolutionIndependentRenderer _resolutionIndependentRenderer;
        private readonly ScreenShaker _screenShaker;
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

        private double _counter;

        private int _currentFragmentIndex;

        private bool _finalBossWasDefeat;
        private int _frameIndex;


        private bool _interactButtonClicked;
        private UnitStatePanelController? _unitStatePanelController;

        public VoiceCombatScreen(EwarGame game) : base(game)
        {
            var soundtrackManager = Game.Services.GetService<SoundtrackManager>();

            _globeProvider = game.Services.GetService<GlobeProvider>();
            _camera = Game.Services.GetService<Camera2D>();

            _globe = _globeProvider.Globe;
            //_currentEventNode = _globe.CurrentDialogue ?? throw new InvalidOperationException();

            _combat = _globe.ActiveCombat ??
                      throw new InvalidOperationException(
                          nameof(_globe.ActiveCombat) + " can't be null in this screen.");

            _globeNode = _combat.Node;
            soundtrackManager.PlayCombatTrack(_globe.ActiveCombat.Node.BiomeType);

            _gameObjects = new List<UnitGameObject>();
            _corpseObjects = new List<CorpseGameObject>();
            _bulletObjects = new List<IInteractionDelivery>();
            _interactionButtons = new List<ButtonBase>();

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
                new Vector2(320, 300),
                new Vector2(290, 250),
                new Vector2(290, 350),
                new Vector2(200, 250),
                new Vector2(200, 350),
                new Vector2(150, 300)
            };

            _screenShaker = new ScreenShaker();
        }

        protected override IList<ButtonBase> CreateMenu()
        {
            return ArraySegment<ButtonBase>.Empty;
        }

        protected override void DrawContentWithoutMenu(SpriteBatch spriteBatch, Rectangle contentRectangle)
        {
            _resolutionIndependentRenderer.BeginDraw();

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
                    _uiContentStorage, _resolutionIndependentRenderer, _globe.Player);
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

        private static void ApplyCombatReward(IReadOnlyCollection<ResourceReward> xpItems, Player player)
        {
            foreach (var item in xpItems)
            {
                var inventoryItem = player.Inventory.Single(x => x.Type == item.Type);

                inventoryItem.Amount += item.Amount;
            }
        }

        private void Combat_ActionGenerated(object? sender, ActionEventArgs e)
        {
            var actor = GetUnitGameObject(e.Actor);
            var target = GetUnitGameObject(e.Target);

            var blocker = _animationManager.CreateAndUseBlocker();

            actor.SkillAnimationCompleted += Actor_SkillAnimationCompleted;

            var bulletBlocker = _animationManager.CreateAndUseBlocker();

            throw new InvalidOperationException();
            //            actor.UseSkill(target, _bulletObjects, e.Skill, e.Action);
        }

        private void Combat_CombatUnitRemoved(object? sender, CombatUnit combatUnit)
        {
            var gameObject = _gameObjects.Single(x => x.CombatUnit == combatUnit);
            _gameObjects.Remove(gameObject);
            combatUnit.HasTakenHitPointsDamage -= CombatUnit_HasTakenDamage;
            combatUnit.HasBeenHitPointsRestored -= CombatUnit_Healed;
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

            _combatSkillsPanel.CombatUnit = null;
            _combatSkillsPanel.SelectedSkill = null;
            _combatSkillsPanel.IsEnabled = false;
        }

        private void Combat_UnitDied(object? sender, CombatUnit e)
        {
            //e.UnscribeHandlers();

            var unitGameObject = GetUnitGameObject(e);

            var corpse = unitGameObject.CreateCorpse();
            _corpseObjects.Add(corpse);
        }

        private void Combat_UnitEntered(object? sender, CombatUnit combatUnit)
        {
            // if (combatUnit.Unit.UnitScheme.IsMonster)
            // {
            //     AddMonstersFromCombatIntoKnownMonsters(combatUnit.Unit, _globe.Player.KnownMonsters);
            // }
            //
            // var position = GetUnitPosition(combatUnit.Index, combatUnit.Unit.IsPlayerControlled);
            // var gameObject =
            //     new UnitGameObject(combatUnit, position, _gameObjectContentStorage, _camera, _screenShaker);
            // _gameObjects.Add(gameObject);
            // combatUnit.HasTakenDamage += CombatUnit_HasTakenDamage;
            // combatUnit.HasBeenHealed += CombatUnit_Healed;
            // combatUnit.HasAvoidedDamage += CombatUnit_HasAvoidedDamage;
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

            var selectedUnit = e;

            _combatSkillsPanel.IsEnabled = true;
            _combatSkillsPanel.CombatUnit = selectedUnit;
            _combatSkillsPanel.SelectedSkill = selectedUnit.CombatCards.First();
            var unitGameObject = GetUnitGameObject(e);
            unitGameObject.IsActive = true;
        }

        private void CombatInitialize()
        {
            _combatSkillsPanel = new CombatSkillPanel(_uiContentStorage.GetButtonTexture(), _uiContentStorage, _combat);
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

            _unitStatePanelController = new UnitStatePanelController(_combat,
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
                var currentCombatList = _globeNode.AssignedCombatSequence.Combats.ToList();
                currentCombatList.Remove(_combat.CombatSource);
                _globeNode.AssignedCombatSequence.Combats = currentCombatList;

                if (_combat.Node.AssignedCombatSequence.Combats.Any())
                {
                    var nextCombat = _globeNode.AssignedCombatSequence.Combats.First();
                    _globe.ActiveCombat = new Core.Combat(_globe.Player.Party,
                        _globeNode,
                        nextCombat,
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

                            _globeProvider.StoreCurrentGlobe();
                        }
                        else
                        {
                            _globeProvider.Globe.UpdateNodes(_dice, _eventCatalog);
                            // _globe.CurrentEventNode = _globe.CurrentEvent.BeforeCombatStartNode;

                            _globe.CurrentEvent.Counter++;

                            ScreenManager.ExecuteTransition(this, ScreenTransition.Event);
                        }
                    }
                    else
                    {
                        if (_globe.CurrentDialogue is null)
                        {
                            _globeProvider.Globe.UpdateNodes(_dice, _eventCatalog);
                            ScreenManager.ExecuteTransition(this, ScreenTransition.Biome);
                            _globeProvider.StoreCurrentGlobe();
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
                _globeProvider.Globe.UpdateNodes(_dice, _eventCatalog);
                ScreenManager.ExecuteTransition(this, ScreenTransition.Biome);
            }
            else
            {
                Debug.Fail("Unknown combat result.");

                RestoreGroupAfterCombat();

                // Fallback is just show biome.
                _globeProvider.Globe.UpdateNodes(_dice, _eventCatalog);
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

        private void CombatUnit_HasTakenDamage(object? sender, UnitStatChangedEventArgs e)
        {
            Debug.Assert(e.CombatUnit is not null);

            if (e.CombatUnit.IsDead)
            {
                return;
            }

            var unitGameObject = GetUnitGameObject(e.CombatUnit);

            var font = _uiContentStorage.GetCombatIndicatorFont();
            var position = unitGameObject.Position;

            var damageIndicator = new HitPointsChangedTextIndicator(-e.Amount, e.Direction, position, font, 1);

            unitGameObject.AddChild(damageIndicator);
        }

        private void CombatUnit_Healed(object? sender, UnitStatChangedEventArgs e)
        {
            Debug.Assert(e.CombatUnit is not null);
            var unitGameObject = GetUnitGameObject(e.CombatUnit);

            var font = _uiContentStorage.GetCombatIndicatorFont();
            var position = unitGameObject.Position;

            var damageIndicator = new HitPointsChangedTextIndicator(e.Amount, e.Direction, position, font, 1);

            unitGameObject.AddChild(damageIndicator);
        }

        private static ResourceReward CreateReward(IReadOnlyCollection<ResourceItem> inventory,
            EquipmentItemType resourceType, int amount)
        {
            var inventoryItem = inventory.Single(x => x.Type == resourceType);
            var item = new ResourceReward
            {
                StartValue = inventoryItem.Amount,
                Amount = amount,
                Type = resourceType
            };

            return item;
        }

        private static ResourceReward CreateXpReward(IReadOnlyCollection<ResourceItem> inventory, int amount)
        {
            const EquipmentItemType EXPERIENCE_POINTS_TYPE = EquipmentItemType.ExperiencePoints;
            return CreateReward(inventory, EXPERIENCE_POINTS_TYPE, amount);
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
            if (_globeNode.AssignedCombatSequence is not null)
            {
                var sumSequenceLength = _globeNode.AssignedCombatSequence.Combats.Count +
                                        _globeNode.AssignedCombatSequence.CompletedCombats.Count;

                var completeCombatCount = _globeNode.AssignedCombatSequence.CompletedCombats.Count + 1;

                var position = new Vector2(_resolutionIndependentRenderer.VirtualBounds.Center.X, 5);

                spriteBatch.DrawString(_uiContentStorage.GetMainFont(),
                    string.Format(UiResource.CombatProgressTemplate, completeCombatCount, sumSequenceLength),
                    position, Color.White);

                spriteBatch.DrawString(_uiContentStorage.GetMainFont(),
                    string.Format(UiResource.MonsterDangerTemplate, _combat.CombatSource.Level),
                    position + new Vector2(0, 10), Color.White);
            }
        }

        private void DrawCombatSkillsPanel(SpriteBatch spriteBatch)
        {
            if (_combatSkillsPanel is not null)
            {
                const int COMBAT_SKILLS_PANEL_WIDTH = 480;
                const int COMBAT_SKILLS_PANEL_HEIGHT = 64;
                _combatSkillsPanel.Rect = new Rectangle(
                    _resolutionIndependentRenderer.VirtualBounds.Center.X - COMBAT_SKILLS_PANEL_WIDTH / 2,
                    _resolutionIndependentRenderer.VirtualBounds.Bottom - COMBAT_SKILLS_PANEL_HEIGHT,
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

            // DrawBullets(spriteBatch);
            //
            // DrawUnits(spriteBatch);
            //
            // foreach (var bullet in _bulletObjects)
            // {
            //     bullet.Draw(spriteBatch);
            // }
            //
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
                DrawCombatSkillsPanel(spriteBatch);
                DrawInteractionButtons(spriteBatch);
            }

            try
            {
                DrawUnitStatePanels(spriteBatch, contentRectangle);
                DrawCombatSequenceProgress(spriteBatch);
                DrawSpeaker(spriteBatch);
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

        private void DrawSpeaker(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(
                sortMode: SpriteSortMode.Deferred,
                blendState: BlendState.AlphaBlend,
                samplerState: SamplerState.PointClamp,
                depthStencilState: DepthStencilState.None,
                rasterizerState: RasterizerState.CullNone,
                transformMatrix: Camera.GetViewTransformationMatrix());

            var col = _frameIndex % 2;
            var row = _frameIndex / 2;

            var currentFragment = _currentEventNode.TextBlock.Fragments[_currentFragmentIndex];
            spriteBatch.Draw(_gameObjectContentStorage.GetCharacterFaceTexture(currentFragment.Speaker),
                new Rectangle(0, ResolutionIndependentRenderer.VirtualHeight - 256, 256, 256),
                new Rectangle(col * 256, row * 256, 256, 256),
                Color.White);

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

        private void DrawUnitStatePanels(SpriteBatch spriteBatch, Rectangle contentRectangle)
        {
            _unitStatePanelController?.Draw(spriteBatch, contentRectangle);
        }

        private UnitGameObject GetUnitGameObject(ICombatUnit combatUnit)
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
                var skillButtonFixedList = _interactionButtons.ToArray();
                foreach (var button in skillButtonFixedList)
                {
                    button.Update(_resolutionIndependentRenderer);
                }

                _combatSkillsPanel?.Update(_resolutionIndependentRenderer);
            }
        }

        private void HandleGlobe(CombatResult result)
        {
            _bossWasDefeat = false;
            _finalBossWasDefeat = false;

            switch (result)
            {
                case CombatResult.Victory:
                    if (_globe.CurrentEvent is not null)
                    {
                        _globe.CurrentEvent.Completed = true;

                        // _globe.CurrentEventNode = _globe.CurrentEvent.AfterCombatStartNode;
                    }

                    break;

                case CombatResult.Defeat:

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

            var rewardList = new List<ResourceReward>();
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
                BiomeProgress = new ResourceReward
                {
                    StartValue = 1,
                    Amount = 1
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
                _combat.UseSkill(skillCard, target.CombatUnit);
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

            var availableTargetGameObjects = _gameObjects.Where(x => !x.CombatUnit.IsDead);
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
                                    !x.CombatUnit.IsDead && !x.CombatUnit.Unit.IsPlayerControlled &&
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
                //unit.RestoreManaPoint();
            }
        }

        private void ShowCombatResultModal(bool isVictory)
        {
            CombatResultModal combatResultModal;

            if (isVictory)
            {
                var completedCombats = _globeNode.AssignedCombatSequence.CompletedCombats;
                completedCombats.Add(_combat.CombatSource);

                var currentCombatList = _combat.Node.AssignedCombatSequence.Combats.ToList();
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
                        _resolutionIndependentRenderer,
                        CombatResult.Victory,
                        xpItems);
                }
                else
                {
                    combatResultModal = new CombatResultModal(
                        _uiContentStorage,
                        _gameObjectContentStorage,
                        _resolutionIndependentRenderer,
                        CombatResult.NextCombat,
                        new CombatRewards
                        {
                            BiomeProgress = new ResourceReward(),
                            InventoryRewards = Array.Empty<ResourceReward>()
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
                    _resolutionIndependentRenderer,
                    CombatResult.Defeat,
                    new CombatRewards
                    {
                        BiomeProgress = new ResourceReward
                        {
                            StartValue = 1,
                            Amount = 1
                        },
                        InventoryRewards = Array.Empty<ResourceReward>()
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