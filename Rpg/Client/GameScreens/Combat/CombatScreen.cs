using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Client;
using Client.Assets.Catalogs;
using Client.Assets.CombatMovements;
using Client.Assets.StoryPointJobs;
using Client.Core;
using Client.Core.Campaigns;
using Client.Engine;
using Client.GameScreens.Campaign;
using Client.GameScreens.Combat;
using Client.GameScreens.Combat.CombatDebugElements;
using Client.GameScreens.Combat.GameObjects;
using Client.GameScreens.Combat.Ui;
using Client.GameScreens.CommandCenter;

using Core.Combats;
using Core.Dices;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Rpg.Client.Core;
using Rpg.Client.Engine;
using Rpg.Client.GameScreens.Combat.GameObjects;
using Rpg.Client.GameScreens.Combat.GameObjects.Background;
using Rpg.Client.GameScreens.Combat.Tutorial;
using Rpg.Client.GameScreens.Combat.Ui;
using Rpg.Client.GameScreens.Common;
using Rpg.Client.ScreenManagement;

using UnitStatType = Core.Combats.UnitStatType;

namespace Rpg.Client.GameScreens.Combat
{
    internal class CombatScreen : GameScreenWithMenuBase
    {
        private const int BACKGROUND_LAYERS_COUNT = 4;
        private const float BACKGROUND_LAYERS_SPEED_X = 0.1f;
        private const float BACKGROUND_LAYERS_SPEED_Y = 0.05f;

        private readonly UpdatableAnimationManager _animationManager;
        private readonly CombatScreenTransitionArguments _args;
        private readonly IList<IInteractionDelivery> _bulletObjects;
        private readonly Camera2D _camera;
        private readonly IReadOnlyCollection<IBackgroundObject> _cloudLayerObjects;
        private readonly CombatCore _combatCore;
        private readonly IList<CorpseGameObject> _corpseObjects;
        private readonly HeroCampaign _currentCampaign;
        private readonly IDice _dice;
        private readonly ICombatMovementVisualizer _combatMovementVisualizer;
        private readonly IEventCatalog _eventCatalog;
        private readonly IReadOnlyList<IBackgroundObject> _farLayerObjects;
        private readonly IReadOnlyList<IBackgroundObject> _foregroundLayerObjects;
        private readonly GameObjectContentStorage _gameObjectContentStorage;
        private readonly IList<CombatantGameObject> _gameObjects;
        private readonly GameSettings _gameSettings;
        private readonly Globe _globe;
        private readonly GlobeNode _globeNode;
        private readonly GlobeProvider _globeProvider;
        private readonly IJobProgressResolver _jobProgressResolver;
        private readonly PlayerCombatActorBehaviour _playerCombatantBehaviour;
        private readonly IReadOnlyList<IBackgroundObject> _mainLayerObjects;
        private readonly ScreenShaker _screenShaker;
        private readonly IUiContentStorage _uiContentStorage;
        private readonly ICombatantPositionProvider _combatantPositionProvider;
        private readonly ICombatActorBehaviourDataProvider _combatDataBehaviourProvider;

        private float _bgCenterOffsetPercentageX;
        private float _bgCenterOffsetPercentageY;

        private bool _bossWasDefeat;
        private double _combatFinishedDelayCounter;

        private bool? _combatFinishedVictory;

        private bool _combatResultModalShown;
        private CombatMovementsHandPanel? _combatMovementsHandPanel;

        private bool _finalBossWasDefeat;

        private UnitStatePanelController? _unitStatePanelController;

        public CombatScreen(TestamentGame game, CombatScreenTransitionArguments args) : base(game)
        {
            _args = args;
            var soundtrackManager = Game.Services.GetService<SoundtrackManager>();

            _globeProvider = game.Services.GetService<GlobeProvider>();
            _camera = Game.Services.GetService<Camera2D>();

            _globe = _globeProvider.Globe;

            _globeNode = args.Location;

            _currentCampaign = args.Campaign;

            _gameObjects = new List<CombatantGameObject>();
            _corpseObjects = new List<CorpseGameObject>();
            _bulletObjects = new List<IInteractionDelivery>();

            _gameObjectContentStorage = game.Services.GetService<GameObjectContentStorage>();
            _uiContentStorage = game.Services.GetService<IUiContentStorage>();
            _animationManager = new UpdatableAnimationManager(new AnimationManager());
            _dice = Game.Services.GetService<IDice>();
            _combatMovementVisualizer = Game.Services.GetRequiredService<ICombatMovementVisualizer>();

            var bgofSelector = Game.Services.GetService<BackgroundObjectFactorySelector>();

            _eventCatalog = game.Services.GetService<IEventCatalog>();

            var backgroundObjectFactory = bgofSelector.GetBackgroundObjectFactory(_globeNode.Sid);

            _cloudLayerObjects = backgroundObjectFactory.CreateCloudLayerObjects();
            _foregroundLayerObjects = backgroundObjectFactory.CreateForegroundLayerObjects();
            _farLayerObjects = backgroundObjectFactory.CreateFarLayerObjects();
            _mainLayerObjects = backgroundObjectFactory.CreateMainLayerObjects();

            _gameSettings = game.Services.GetService<GameSettings>();

            _combatantPositionProvider = new CombatantPositionProvider(ResolutionIndependentRenderer.VirtualWidth);

            _screenShaker = new ScreenShaker();

            _jobProgressResolver = new JobProgressResolver();

            _playerCombatantBehaviour = new PlayerCombatActorBehaviour();

            _combatCore = CreateCombat();
            _combatDataBehaviourProvider = new CombatActorBehaviourDataProvider(_combatCore);

            soundtrackManager.PlayCombatTrack((BiomeType)((int)args.Location.Sid / 100 * 100));
        }

        protected override IList<ButtonBase> CreateMenu()
        {
            var surrenderButton = new ResourceTextButton(nameof(UiResource.SurrenderButtonTitle));
            surrenderButton.OnClick += EscapeButton_OnClick;

            return new ButtonBase[] { surrenderButton };
        }

        protected override void DrawContentWithoutMenu(SpriteBatch spriteBatch, Rectangle contentRectangle)
        {
            ResolutionIndependentRenderer.BeginDraw();

            DrawGameObjects(spriteBatch);

            DrawHud(spriteBatch, contentRectangle);
        }

        protected override void InitializeContent()
        {
            CombatInitialize();
        }

        protected override void UpdateContent(GameTime gameTime)
        {
            base.UpdateContent(gameTime);

            var keyboard = Keyboard.GetState();
            _gameSettings.IsRecordMode = keyboard.GetPressedKeys().Contains(Keys.Z);

            if (!_globe.Player.HasAbility(PlayerAbility.ReadCombatTutorial) &&
                !_globe.Player.HasAbility(PlayerAbility.SkipTutorials))
            {
                _globe.Player.AddPlayerAbility(PlayerAbility.ReadCombatTutorial);
                var tutorialModal = new TutorialModal(new CombatTutorialPageDrawer(_uiContentStorage),
                    _uiContentStorage, ResolutionIndependentRenderer, _globe.Player);
                AddModal(tutorialModal, isLate: false);
            }

            UpdateBackgroundObjects(gameTime);

            HandleBullets(gameTime);

            HandleUnits(gameTime);

            if (!_combatCore.Finished && _combatFinishedVictory is null)
            {
                UpdateCombatHud();
            }

            _screenShaker.Update(gameTime);

            HandleBackgrounds();

            if (_combatFinishedVictory is not null)
            {
                UpdateCombatFinished(gameTime);
            }

            _animationManager.Update(gameTime.ElapsedGameTime.TotalSeconds);
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

        private static void ApplyCombatReward(IReadOnlyCollection<ResourceReward> xpItems, Player player)
        {
            foreach (var item in xpItems)
            {
                var inventoryItem = player.Inventory.Single(x => x.Type == item.Type);

                inventoryItem.Amount += item.Amount;
            }
        }

        private static CombatRewards CalculateRewardGaining(
            IEnumerable<CombatSource> completedCombats,
            GlobeNode globeNode,
            Player player,
            GlobeLevel globeLevel)
        {
            var completedCombatsShortInfos = completedCombats.Select(x =>
                new CombatRewardInfo(x.EnemyGroup.GetUnits()
                    .Select(enemy => new CombatMonsterRewardInfo(enemy.XpReward))));

            var rewardCalculationContext = new RewardCalculationContext(
                player.Inventory,
                globeNode.EquipmentItem,
                completedCombatsShortInfos,
                globeLevel.Level,
                15
            );

            var rewards = CombatScreenHelper.CalculateRewards(rewardCalculationContext);

            return rewards;
        }

        private void CombatCode_CombatantHasBeenDefeated(object? sender, CombatantDefeatedEventArgs e)
        {
            if (!e.Combatant.IsPlayerControlled)
            {
                CountDefeat();
            }

            var combatantGameObject = GetCombatantGameObject(e.Combatant);

            var corpse = combatantGameObject.CreateCorpse();
            _corpseObjects.Add(corpse);
            
            _gameObjects.Remove(combatantGameObject);
        }

        private void Combat_Finish(object? sender, CombatFinishEventArgs e)
        {
            _combatMovementsHandPanel = null;

            _combatFinishedVictory = e.Victory;

            CountCombatFinished();

            // See UpdateCombatFinished next
        }

        private void CombatCore_CombatantEndsTurn(object? sender, CombatantEndsTurnEventArgs e)
        {
            DropSelection(e.Combatant);
            
            if (_combatMovementsHandPanel is not null)
            {
                _combatMovementsHandPanel.Combatant = null;
                _combatMovementsHandPanel.IsEnabled = false;
            }
        }

        private void CombatCore_CombatantStartsTurn(object? sender, CombatantTurnStartedEventArgs e)
        {
            if (_combatMovementsHandPanel is not null && _combatCore.CurrentCombatant.IsPlayerControlled)
            {
                _combatMovementsHandPanel.IsEnabled = true;
                _combatMovementsHandPanel.Combatant = e.Combatant;
            }

            var behaviourData = _combatDataBehaviourProvider.GetDataSnapshot();

            _combatCore.CurrentCombatant.Behaviour.HandleIntention(behaviourData,
                intention =>
                {
                    intention.Make(_combatCore);
                });
        }

        private void CombatCode_CombatantHasBeenAdded(object? sender, CombatantHasBeenAddedEventArgs e)
        {
            // Move it to separate handler with core logic. There is only game objects.
            // if (combatUnit.Unit.UnitScheme.IsMonster)
            // {
            //     AddMonstersFromCombatIntoKnownMonsters(combatUnit.Unit, _globe.Player.KnownMonsters);
            // }

            var unitCatalog = Game.Services.GetRequiredService<IUnitGraphicsCatalog>();
            var graphicConfig = unitCatalog.GetGraphics(e.Combatant.ClassSid);


            var combatantSide = e.FieldInfo.FieldSide == _combatCore.Field.HeroSide ? CombatantPositionSide.Heroes : CombatantPositionSide.Monsters;
            var gameObject =
                new CombatantGameObject(e.Combatant, graphicConfig, e.FieldInfo.CombatantCoords, _combatantPositionProvider, _gameObjectContentStorage, _camera, _screenShaker, combatantSide);
            _gameObjects.Add(gameObject);

            // var combatant = e.Combatant;
            //
            // combatUnit.HasTakenHitPointsDamage += CombatUnit_HasTakenHitPointsDamage;
            // combatUnit.HasTakenShieldPointsDamage += CombatUnit_HasTakenShieldPointsDamage;
            // combatUnit.HasBeenHitPointsRestored += CombatUnit_HasBeenHitPointsRestored;
            // combatUnit.HasBeenShieldPointsRestored += CombatUnit_HasBeenShieldPointsRestored;
            // combatUnit.HasAvoidedDamage += CombatUnit_HasAvoidedDamage;
            // combatUnit.Blocked += CombatUnit_Blocked;
        }

        // private void Combat_UnitPassed(object? sender, CombatUnit e)
        // {
        //     var unitGameObject = GetUnitGameObject(e);
        //     var textPosition = GetUnitGameObject(e).Position;
        //     var font = _uiContentStorage.GetCombatIndicatorFont();
        //
        //     var passIndicator = new SkipTextIndicator(textPosition, font);
        //
        //     unitGameObject.AddChild(passIndicator);
        // }

        private void CombatInitialize()
        {
            _combatCore.CombatantHasBeenAdded += CombatCode_CombatantHasBeenAdded;
            _combatCore.CombatantHasBeenDefeated += CombatCode_CombatantHasBeenDefeated;
            _combatCore.CombatantHasBeenDamaged += CombatCore_CombatantHasBeenDamaged;
            _combatCore.CombatantStartsTurn += CombatCore_CombatantStartsTurn;
            _combatCore.CombatantEndsTurn += CombatCore_CombatantEndsTurn;
            _combatCore.CombatantHasBeenMoved += CombatCore_CombatantHasBeenMoved;

            // _combatCore.CombatantHasBeenDamaged += CombatCore_CombatantHasBeenDamaged;
            // _combatCore.CombatantHasBeenDefeated += CombatCore_CombatantHasBeenDefeated;
            // _combatCore.CombatantStartsTurn += CombatCore_CombatantStartsTurn;
            // _combatCore.CombatantEndsTurn += CombatCore_CombatantEndsTurn;
            //
            // _combatCore.ActiveCombatUnitChanged += Combat_UnitChanged;
            // _combatCore.CombatUnitIsReadyToControl += Combat_UnitReadyToControl;
            // _combatCore.CombatUnitRemoved += Combat_CombatUnitRemoved;
            // _combatCore.UnitDied += Combat_UnitDied;
            // _combatCore.ActionGenerated += Combat_ActionGenerated;
            // _combatCore.Finish += Combat_Finish;
            // _combatCore.UnitPassedTurn += Combat_UnitPassed;

            _combatMovementsHandPanel = new CombatMovementsHandPanel(_uiContentStorage);
            _combatMovementsHandPanel.CombatMovementPicked += CombatMovementsHandPanel_CombatMovementPicked;

            _combatCore.Initialize(
                CombatantFactory.CreateHeroes(_playerCombatantBehaviour),
                CombatantFactory.CreateMonsters(new BotCombatActorBehaviour(_animationManager, _combatMovementVisualizer, _gameObjects)));
            
            _unitStatePanelController = new UnitStatePanelController(_combatCore,
                _uiContentStorage, _gameObjectContentStorage);
        }

        private void CombatCore_CombatantHasBeenMoved(object? sender, CombatantHasBeenMovedEventArgs e)
        {
            var newWorldPosition = _combatantPositionProvider.GetPosition(e.NewFieldCoords,
                e.FieldSide == _combatCore.Field.HeroSide
                    ? CombatantPositionSide.Heroes
                    : CombatantPositionSide.Monsters);

            var combatantGameObject = GetCombatantGameObject(e.Combatant);
            combatantGameObject.MoveToFieldCoords(newWorldPosition);
        }

        private void CombatResultModal_Closed(object? sender, EventArgs e)
        {
            _animationManager.DropBlockers();

            if (sender is null)
            {
                throw new InvalidOperationException("Handler must be assigned to object instance instead static.");
            }

            var combatResultModal = (CombatResultModal)sender;

            if (combatResultModal.CombatResult is CombatResult.Victory or CombatResult.NextCombat)
            {
                var nextCombatIndex = _args.CurrentCombatIndex + 1;
                var areAllCombatsWon = nextCombatIndex >= _args.CombatSequence.Combats.Count;
                if (!areAllCombatsWon)
                {
                    var combatScreenArgs = new CombatScreenTransitionArguments(_currentCampaign,
                        _args.CombatSequence,
                        nextCombatIndex,
                        _args.IsAutoplay,
                        _globeNode,
                        _args.VictoryDialogue);

                    ScreenManager.ExecuteTransition(this, ScreenTransition.Combat, combatScreenArgs);
                }
                else
                {
                    RestoreGroupAfterCombat();

                    if (_bossWasDefeat)
                    {
                        if (_finalBossWasDefeat)
                        {
                            ScreenManager.ExecuteTransition(this, ScreenTransition.EndGame, null);

                            if (_gameSettings.Mode == GameMode.Full)
                            {
                                _globeProvider.StoreCurrentGlobe();
                            }
                        }
                        else
                        {
                            _globeProvider.Globe.Update(_dice, _eventCatalog);
                        }
                    }
                    else
                    {
                        _globeProvider.Globe.Update(_dice, _eventCatalog);
                        _currentCampaign.CompleteCurrentStage();
                        ScreenManager.ExecuteTransition(this, ScreenTransition.Campaign,
                            new CampaignScreenTransitionArguments(_currentCampaign));

                        if (_gameSettings.Mode == GameMode.Full)
                        {
                            _globeProvider.StoreCurrentGlobe();
                        }
                    }
                }
            }
            else if (combatResultModal.CombatResult == CombatResult.Defeat)
            {
                RestoreGroupAfterCombat();

                var campaignGenerator = Game.Services.GetService<ICampaignGenerator>();
                var campaigns = campaignGenerator.CreateSet();

                ScreenManager.ExecuteTransition(this, ScreenTransition.CampaignSelection,
                    new CommandCenterScreenTransitionArguments
                    {
                        AvailableCampaigns = campaigns
                    });
            }
            else
            {
                Debug.Fail("Unknown combat result.");

                RestoreGroupAfterCombat();

                // Fallback is just showing of new campaign selection.
                _globeProvider.Globe.Update(_dice, _eventCatalog);

                var campaignGenerator = Game.Services.GetService<ICampaignGenerator>();
                var campaigns = campaignGenerator.CreateSet();

                ScreenManager.ExecuteTransition(this, ScreenTransition.CampaignSelection,
                    new CommandCenterScreenTransitionArguments
                    {
                        AvailableCampaigns = campaigns
                    });
            }
        }

        private void CombatMovementsHandPanel_CombatMovementPicked(object? sender, CombatMovementPickedEventArgs e)
        {
            var intention = new UseCombatMovementIntention(e.CombatMovement, _animationManager, _combatMovementVisualizer, _gameObjects);

            _playerCombatantBehaviour.Assign(intention);
        }

        

        private void CombatCore_CombatantHasBeenDamaged(object? sender, CombatantDamagedEventArgs e)
        {
            if (e.Combatant.IsDead)
            {
                return;
            }

            var unitGameObject = GetCombatantGameObject(e.Combatant);

            unitGameObject.AnimateWound();

            if (!_gameSettings.IsRecordMode)
            {
                var font = _uiContentStorage.GetCombatIndicatorFont();
                var position = unitGameObject.Position;

                if (e.Value <= 0)
                {
                    var blockIndicator = new BlockAnyDamageTextIndicator(position, font);
                    unitGameObject.AddChild(blockIndicator);
                }

                var nextIndex = GetIndicatorNextIndex(unitGameObject);

                switch (e.StatType)
                {
                    case UnitStatType.HitPoints:
                        var damageIndicator =
                            new HitPointsChangedTextIndicator(-e.Value,
                                HitPointsChangeDirection.Negative,
                                position,
                                font,
                                nextIndex ?? 0);

                        unitGameObject.AddChild(damageIndicator);

                        unitGameObject.AnimateWound();

                        break;

                    case UnitStatType.ShieldPoints:
                        var spIndicator =
                            new ShieldPointsChangedTextIndicator(-e.Value,
                                HitPointsChangeDirection.Negative,
                                position,
                                font,
                                nextIndex ?? 0);

                        unitGameObject.AddChild(spIndicator);
                        break;
                }
            }
        }

        private void CountCombatFinished()
        {
            var progress = new CombatCompleteJobProgress();
            var activeStoryPointsSnapshotList = _globe.ActiveStoryPoints.ToArray();
            foreach (var storyPoint in activeStoryPointsSnapshotList)
            {
                _jobProgressResolver.ApplyProgress(progress, storyPoint);
            }
        }

        private void CountDefeat()
        {
            var progress = new DefeatJobProgress();
            var activeStoryPointsMaterialized = _globe.ActiveStoryPoints.ToArray();
            foreach (var storyPoint in activeStoryPointsMaterialized)
            {
                _jobProgressResolver.ApplyProgress(progress, storyPoint);
            }
        }

        private CombatCore CreateCombat()
        {
            return new CombatCore(_dice);
        }
        

        private void DrawBackgroundLayers(SpriteBatch spriteBatch, IReadOnlyList<Texture2D> backgrounds,
            int backgroundStartOffsetX,
            int backgroundMaxOffsetX, int backgroundStartOffsetY, int backgroundMaxOffsetY)
        {
            for (var i = 0; i < BACKGROUND_LAYERS_COUNT; i++)
            {
                var xFloat = backgroundStartOffsetX + _bgCenterOffsetPercentageX * (BACKGROUND_LAYERS_COUNT - i - 1) *
                    BACKGROUND_LAYERS_SPEED_X * backgroundMaxOffsetX;
                var roundedX = (int)Math.Round(xFloat);

                var yFloat = backgroundStartOffsetY + _bgCenterOffsetPercentageY * (BACKGROUND_LAYERS_COUNT - i - 1) *
                    BACKGROUND_LAYERS_SPEED_Y * backgroundMaxOffsetY;
                var roundedY = (int)Math.Round(yFloat);

                var position = new Vector2(roundedX, roundedY);
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
                else if (i == 2 /* Far layer */)
                {
                    foreach (var obj in _farLayerObjects)
                    {
                        obj.Draw(spriteBatch);
                    }
                }
                else if (i == 3) // Main layer
                {
                    foreach (var obj in _mainLayerObjects)
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
            if (_globeNode.AssignedCombats is not null)
            {
                var sumSequenceLength = _globeNode.AssignedCombats.Combats.Count;

                var completeCombatCount = _globeNode.AssignedCombats.CompletedCombats.Count + 1;

                var position = new Vector2(ResolutionIndependentRenderer.VirtualBounds.Center.X, 5);

                spriteBatch.DrawString(_uiContentStorage.GetMainFont(),
                    string.Format(UiResource.CombatProgressTemplate, completeCombatCount, sumSequenceLength),
                    position, Color.White);

                spriteBatch.DrawString(_uiContentStorage.GetMainFont(),
                    string.Format(UiResource.MonsterDangerTemplate, 1),
                    position + new Vector2(0, 10), Color.White);
            }
        }

        private void DrawCombatSkillsPanel(SpriteBatch spriteBatch, Rectangle contentRectangle)
        {
            if (_combatMovementsHandPanel is not null)
            {
                const int COMBAT_SKILLS_PANEL_WIDTH = 480;
                const int COMBAT_SKILLS_PANEL_HEIGHT = 64;
                _combatMovementsHandPanel.Rect = new Rectangle(
                    contentRectangle.Center.X - COMBAT_SKILLS_PANEL_WIDTH / 2,
                    contentRectangle.Bottom - COMBAT_SKILLS_PANEL_HEIGHT,
                    COMBAT_SKILLS_PANEL_WIDTH, COMBAT_SKILLS_PANEL_HEIGHT);
                _combatMovementsHandPanel.Draw(spriteBatch);
            }
        }

        private void DrawForegroundLayers(SpriteBatch spriteBatch, Texture2D[] backgrounds, int backgroundStartOffsetX,
            int backgroundMaxOffsetX, int backgroundStartOffsetY, int backgroundMaxOffsetY)
        {
            var xFloat = backgroundStartOffsetX + _bgCenterOffsetPercentageX * (-1) *
                BACKGROUND_LAYERS_SPEED_X * backgroundMaxOffsetX;
            var roundedX = (int)Math.Round(xFloat);

            var yFloat = backgroundStartOffsetY + _bgCenterOffsetPercentageY * (-1) *
                BACKGROUND_LAYERS_SPEED_Y * backgroundMaxOffsetY;
            var roundedY = (int)Math.Round(yFloat);

            var position = new Vector2(roundedX, roundedY);
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

            spriteBatch.Draw(backgrounds[4], Vector2.Zero, Color.White);

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

            const int BG_START_OFFSET_X = -100;
            const int BG_MAX_OFFSET_X = 200;
            const int BG_START_OFFSET_Y = -20;
            const int BG_MAX_OFFSET_Y = 40;

            DrawBackgroundLayers(spriteBatch, backgrounds, BG_START_OFFSET_X, BG_MAX_OFFSET_X, BG_START_OFFSET_Y,
                BG_MAX_OFFSET_Y);

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

            DrawForegroundLayers(spriteBatch, backgrounds, BG_START_OFFSET_X, BG_MAX_OFFSET_X, BG_START_OFFSET_Y,
                BG_MAX_OFFSET_Y);
        }

        private void DrawHud(SpriteBatch spriteBatch, Rectangle contentRectangle)
        {
            if (_gameSettings.IsRecordMode)
            {
                return;
            }

            spriteBatch.Begin(sortMode: SpriteSortMode.Deferred,
                blendState: BlendState.AlphaBlend,
                samplerState: SamplerState.PointClamp,
                depthStencilState: DepthStencilState.None,
                rasterizerState: RasterizerState.CullNone,
                transformMatrix: _camera.GetViewTransformationMatrix());

            if (_combatCore.CurrentCombatant.IsPlayerControlled == true && !_animationManager.HasBlockers)
            {
                DrawCombatSkillsPanel(spriteBatch, contentRectangle);
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

        private void DropSelection(Combatant combatant)
        {
            var oldCombatUnitGameObject = GetCombatantGameObject(combatant);
            oldCombatUnitGameObject.IsActive = false;
        }

        private void EscapeButton_OnClick(object? sender, EventArgs e)
        {
            //_combatCore.Surrender();
            _combatFinishedVictory = false;
        }

        private static int? GetIndicatorNextIndex(CombatantGameObject unitGameObject)
        {
            var currentIndex = unitGameObject.GetCurrentIndicatorIndex();
            var nextIndex = currentIndex + 1;
            return nextIndex;
        }

        private int GetUnbreakableLevel()
        {
            // TODO Like in How wants to be a millionaire?
            // The reaching of some of levels gains unbreakable level.
            return 0;
        }

        private CombatantGameObject GetCombatantGameObject(Combatant combatant)
        {
            return _gameObjects.First(x => x.Combatant == combatant);
        }

        private void HandleBackgrounds()
        {
            var mouse = Mouse.GetState();
            var mouseRir = ResolutionIndependentRenderer.ScaleMouseToScreenCoordinates(new Vector2(mouse.X, mouse.Y));

            var screenCenterX = ResolutionIndependentRenderer.VirtualBounds.Center.X;
            var rawPercentageX = (mouseRir.X - screenCenterX) / screenCenterX;
            _bgCenterOffsetPercentageX = NormalizePercentage(rawPercentageX);

            var screenCenterY = ResolutionIndependentRenderer.VirtualBounds.Center.Y;
            var rawPercentageY = (mouseRir.Y - screenCenterY) / screenCenterY;
            _bgCenterOffsetPercentageY = NormalizePercentage(rawPercentageY);
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

        private void UpdateCombatHud()
        {
            //if (!_interactButtonClicked)
            {
                _combatMovementsHandPanel?.Update(ResolutionIndependentRenderer);

                _unitStatePanelController?.Update(ResolutionIndependentRenderer);
            }
        }

        private void HandleGlobe(CombatResult result)
        {
            _bossWasDefeat = false;
            _finalBossWasDefeat = false;

            switch (result)
            {
                case CombatResult.Victory:
                    HandleGlobeVictoryResult();
                    break;

                case CombatResult.Defeat:
                    HandleGlobeDefeatResult();
                    break;

                default:
                    throw new InvalidOperationException($"Unknown combat result {result}.");
            }
        }

        private void HandleGlobeDefeatResult()
        {
            var minLevel = GetUnbreakableLevel();
            var levelDiff = _globe.GlobeLevel.Level - minLevel;
            _globe.GlobeLevel.Level = minLevel + levelDiff / 2;
        }

        private void HandleGlobeVictoryResult()
        {
            _globe.GlobeLevel.Level++;
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

        private static float NormalizePercentage(float value)
        {
            return value switch
            {
                < -1 => -1,
                > 1 => 1,
                _ => value
            };
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
                var completedCombats = new List<CombatSource>() { _args.CombatSequence.Combats.First() };

                var isAllCombatSequenceComplete = true;
                if (isAllCombatSequenceComplete)
                {
                    // End the combat sequence
                    var rewardItems = CalculateRewardGaining(completedCombats, _globeNode,
                        _globeProvider.Globe.Player,
                        _globeProvider.Globe.GlobeLevel);
                    ApplyCombatReward(rewardItems.InventoryRewards, _globeProvider.Globe.Player);
                    HandleGlobe(CombatResult.Victory);

                    var soundtrackManager = Game.Services.GetService<SoundtrackManager>();
                    soundtrackManager.PlayVictoryTrack();

                    combatResultModal = new CombatResultModal(
                        _uiContentStorage,
                        _gameObjectContentStorage,
                        ResolutionIndependentRenderer,
                        CombatResult.Victory,
                        rewardItems);
                }
                else
                {
                    // Next combat

                    combatResultModal = new CombatResultModal(
                        _uiContentStorage,
                        _gameObjectContentStorage,
                        ResolutionIndependentRenderer,
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
                    ResolutionIndependentRenderer,
                    CombatResult.Defeat,
                    new CombatRewards
                    {
                        BiomeProgress = new ResourceReward
                        {
                            StartValue = _globe.GlobeLevel.Level,
                            Amount = _globe.GlobeLevel.Level / 2
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

            foreach (var obj in _mainLayerObjects)
            {
                obj.Update(gameTime);
            }

            foreach (var obj in _farLayerObjects)
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
            _combatFinishedDelayCounter += gameTime.ElapsedGameTime.TotalSeconds;

            if (_combatFinishedDelayCounter >= 2 && !_combatResultModalShown)
            {
                _combatResultModalShown = true;
                ShowCombatResultModal(_combatFinishedVictory.Value);
            }
        }
    }

    internal sealed class UseCombatMovementIntention : IIntention
    {
        private readonly CombatMovementInstance _combatMovement;
        private readonly IAnimationManager _animationManager;
        private readonly ICombatMovementVisualizer _combatMovementVisualizer;
        private readonly IList<CombatantGameObject> _combatantGameObjects;

        public UseCombatMovementIntention(CombatMovementInstance combatMovement, IAnimationManager animationManager, ICombatMovementVisualizer combatMovementVisualizer, IList<CombatantGameObject> combatantGameObjects)
        {
            _combatMovement = combatMovement;
            _animationManager = animationManager;
            _combatMovementVisualizer = combatMovementVisualizer;
            _combatantGameObjects = combatantGameObjects;
        }

        public void Make(CombatCore combatCore)
        {
            var movementExecution = combatCore.CreateCombatMovementExecution(_combatMovement);

            var actorGameObject = GetCombatantGameObject(combatCore.CurrentCombatant);
            var movementState = GetMovementVisualizationState(actorGameObject, movementExecution, _combatMovement);

            PlaybackCombatMovementExecution(movementState, combatCore);
        }

        private void PlaybackCombatMovementExecution(IActorVisualizationState movementState, CombatCore combatCore)
        {
            var actorGameObject = GetCombatantGameObject(combatCore.CurrentCombatant);

            var mainAnimationBlocker = _animationManager.CreateAndRegisterBlocker();

            mainAnimationBlocker.Released += (sender, args) =>
            {
                // Wait some time to separate turns of different actors

                var delayBlocker = new DelayBlocker(new Duration(1));
                _animationManager.RegisterBlocker(delayBlocker);

                delayBlocker.Released += (_, _) =>
                {
                    combatCore.CompleteTurn();
                };
            };

            var actorState = new SequentialState(
                // Delay to focus on the current actor.
                new DelayActorState(new Duration(0.75f)),

                // Main move animation.
                movementState,

                // Release the main animation blocker to say the main move is ended.
                new AnimationBlockerTerminatorActorState(mainAnimationBlocker));

            actorGameObject.AddStateEngine(actorState);
        }

        private IActorVisualizationState GetMovementVisualizationState(CombatantGameObject actorGameObject, CombatMovementExecution movementExecution, CombatMovementInstance combatMovement)
        {
            var context = new CombatMovementVisualizationContext(_combatantGameObjects.ToArray());

            return _combatMovementVisualizer.GetMovementVisualizationState(combatMovement.SourceMovement.Sid, actorGameObject.Animator, movementExecution, context);
        }

        private CombatantGameObject GetCombatantGameObject(Combatant combatant)
        {
            return _combatantGameObjects.First(x => x.Combatant == combatant);
        }
    }

    internal sealed class PlayerCombatActorBehaviour : ICombatActorBehaviour
    {
        private Action<IIntention>? _intentionDelegate;

        public void Assign(IIntention intention)
        {
            if (_intentionDelegate is null)
            {
                throw new InvalidOperationException();
            }

            _intentionDelegate(intention);
        }

        public void HandleIntention(ICombatActorBehaviourData combatData, Action<IIntention> intentionDelegate)
        {
            _intentionDelegate = intentionDelegate;
        }
    }

    internal sealed class BotCombatActorBehaviour : ICombatActorBehaviour
    {
        private readonly IAnimationManager _animationManager;
        private readonly ICombatMovementVisualizer _combatMovementVisualizer;
        private readonly IList<CombatantGameObject> _combatantGameObjects;

        public BotCombatActorBehaviour(IAnimationManager animationManager, ICombatMovementVisualizer combatMovementVisualizer, IList<CombatantGameObject> combatantGameObjects)
        {
            _animationManager = animationManager;
            _combatMovementVisualizer = combatMovementVisualizer;
            _combatantGameObjects = combatantGameObjects;
        }

        public void HandleIntention(ICombatActorBehaviourData combatData, Action<IIntention> intentionDelegate)
        {
            var firstSkill = combatData.CurrentActor.Skills.First();

            var skillIntention = new UseCombatMovementIntention(firstSkill.CombatMovement, _animationManager, _combatMovementVisualizer, _combatantGameObjects);

            intentionDelegate(skillIntention);
        }
    }

    public sealed class CombatActorBehaviourDataProvider : ICombatActorBehaviourDataProvider
    {
        private readonly CombatCore _combat;

        public CombatActorBehaviourDataProvider(CombatCore combat)
        {
            _combat = combat;
        }

        public ICombatActorBehaviourData GetDataSnapshot()
        {
            return new CombatUnitBehaviourData(_combat);
        }
    }

    public sealed class CombatUnitBehaviourData : ICombatActorBehaviourData
    {
        public CombatUnitBehaviourData(CombatCore combat)
        {
            CurrentActor =
                new CombatUnitBehaviourDataActor(
                    combat.CurrentCombatant.Hand.Where(x=>x is not null).Select(skill => new CombatActorBehaviourDataSkill(skill!)).ToArray());

            Actors = combat.Field.HeroSide.GetAllCombatants().Concat(combat.Field.MonsterSide.GetAllCombatants()).Where(actor => actor != combat.CurrentCombatant).Select(actor =>
                    new CombatUnitBehaviourDataActor(
                        actor.Hand.Where(x => x is not null).Select(skill => new CombatActorBehaviourDataSkill(skill!)).ToArray()))
                .ToArray();
        }

        public CombatUnitBehaviourDataActor CurrentActor { get; }
        public IReadOnlyCollection<CombatUnitBehaviourDataActor> Actors { get; }
    }
}