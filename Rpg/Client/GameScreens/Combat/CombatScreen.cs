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
using Core.Combats.BotBehaviour;
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

namespace Rpg.Client.GameScreens.Combat;

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
    private readonly ICombatantPositionProvider _combatantPositionProvider;
    private readonly CombatCore _combatCore;
    private readonly ICombatActorBehaviourDataProvider _combatDataBehaviourProvider;
    private readonly ICombatMovementVisualizer _combatMovementVisualizer;
    private readonly IList<CorpseGameObject> _corpseObjects;
    private readonly HeroCampaign _currentCampaign;
    private readonly IDice _dice;
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
    private readonly IReadOnlyList<IBackgroundObject> _mainLayerObjects;
    private readonly FieldManeuverIndicatorPanel _maneuversIndicator;
    private readonly FieldManeuversVisualizer _maneuversVisualizer;
    private readonly PlayerCombatActorBehaviour _playerCombatantBehaviour;
    private readonly ScreenShaker _screenShaker;
    private readonly IUiContentStorage _uiContentStorage;

    private float _bgCenterOffsetPercentageX;
    private float _bgCenterOffsetPercentageY;

    private bool _bossWasDefeat;
    private double _combatFinishedDelayCounter;

    private bool? _combatFinishedVictory;
    private CombatMovementsHandPanel? _combatMovementsHandPanel;

    private bool _combatResultModalShown;

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

        _maneuversVisualizer =
            new FieldManeuversVisualizer(_combatantPositionProvider, new ManeuverContext(_combatCore));

        _maneuversIndicator = new FieldManeuverIndicatorPanel(UiThemeManager.UiContentStorage.GetTitlesFont(),
            new ManeuverContext(_combatCore));
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

        _maneuversVisualizer.ManeuverSelected += ManeuverVisualizer_ManeuverSelected;
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
            UpdateCombatHud(gameTime);
        }

        _screenShaker.Update(gameTime);

        HandleBackgrounds();

        if (_combatFinishedVictory is not null)
        {
            UpdateCombatFinished(gameTime);
        }

        _animationManager.Update(gameTime.ElapsedGameTime.TotalSeconds);
    }

    //private static void AddMonstersFromCombatIntoKnownMonsters(Client.Core.Heroes.Hero monster,
    //    ICollection<UnitScheme> playerKnownMonsters)
    //{
    //    var scheme = monster.UnitScheme;
    //    if (playerKnownMonsters.All(x => x != scheme))
    //    {
    //        playerKnownMonsters.Add(scheme);
    //    }
    //}

    private static void ApplyCombatReward(IReadOnlyCollection<ResourceReward> xpItems, Player player)
    {
        foreach (var item in xpItems)
        {
            var inventoryItem = player.Inventory.Single(x => x.Type == item.Type);

            inventoryItem.Amount += item.Amount;
        }
    }

    private CombatStepDirection? CalcDirection(Combatant combatant, FieldCoords targetCoords)
    {
        var combatantCoords = _combatCore.Field.HeroSide.GetCombatantCoords(combatant);

        var lineDiff = targetCoords.LineIndex - combatantCoords.LineIndex;
        var columnDiff = targetCoords.ColumentIndex - combatantCoords.ColumentIndex;

        if (columnDiff > 0 && lineDiff == 0)
        {
            return CombatStepDirection.Backward;
        }

        if (columnDiff < 0 && lineDiff == 0)
        {
            return CombatStepDirection.Forward;
        }

        if (columnDiff == 0 && lineDiff < 0)
        {
            return CombatStepDirection.Up;
        }

        if (columnDiff == 0 && lineDiff > 0)
        {
            return CombatStepDirection.Down;
        }

        return null;
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

    private void CombatCode_CombatantHasBeenAdded(object? sender, CombatantHasBeenAddedEventArgs e)
    {
        // Move it to separate handler with core logic. There is only game objects.
        // if (combatUnit.Unit.UnitScheme.IsMonster)
        // {
        //     AddMonstersFromCombatIntoKnownMonsters(combatUnit.Unit, _globe.Player.KnownMonsters);
        // }

        var unitCatalog = Game.Services.GetRequiredService<IUnitGraphicsCatalog>();
        var graphicConfig = unitCatalog.GetGraphics(e.Combatant.ClassSid);

        var combatantSide = e.FieldInfo.FieldSide == _combatCore.Field.HeroSide
            ? CombatantPositionSide.Heroes
            : CombatantPositionSide.Monsters;
        var gameObject =
            new CombatantGameObject(e.Combatant, graphicConfig, e.FieldInfo.CombatantCoords, _combatantPositionProvider,
                _gameObjectContentStorage, _camera, _screenShaker, combatantSide);
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

    private void CombatCore_CombatantEndsTurn(object? sender, CombatantEndsTurnEventArgs e)
    {
        DropSelection(e.Combatant);

        if (_combatMovementsHandPanel is not null)
        {
            _combatMovementsHandPanel.Combatant = null;
            _combatMovementsHandPanel.IsEnabled = false;
        }

        _maneuversVisualizer.Combatant = null;
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

    private void CombatCore_CombatantHasBeenMoved(object? sender, CombatantHasBeenMovedEventArgs e)
    {
        var newWorldPosition = _combatantPositionProvider.GetPosition(e.NewFieldCoords,
            e.FieldSide == _combatCore.Field.HeroSide
                ? CombatantPositionSide.Heroes
                : CombatantPositionSide.Monsters);

        var combatantGameObject = GetCombatantGameObject(e.Combatant);
        combatantGameObject.MoveToFieldCoords(newWorldPosition);
    }

    private void CombatCore_CombatantStartsTurn(object? sender, CombatantTurnStartedEventArgs e)
    {
        if (_combatMovementsHandPanel is not null && _combatCore.CurrentCombatant.IsPlayerControlled)
        {
            _combatMovementsHandPanel.IsEnabled = true;
            _combatMovementsHandPanel.Combatant = e.Combatant;
        }

        _maneuversVisualizer.Combatant = e.Combatant;

        var behaviourData = _combatDataBehaviourProvider.GetDataSnapshot();

        _combatCore.CurrentCombatant.Behaviour.HandleIntention(behaviourData,
            intention =>
            {
                intention.Make(_combatCore);
            });
    }

    private void CombatCore_CombatantUsedMove(object? sender, CombatantHandChangedEventArgs e)
    {
        if (e.Combatant.IsPlayerControlled)
        {
            _combatMovementsHandPanel?.StartMovementBurning(e.HandSlotIndex);
        }
    }

    private void CombatCore_CombatFinished(object? sender, CombatFinishedEventArgs e)
    {
        _combatMovementsHandPanel = null;

        _combatFinishedVictory = e.Result == CombatFinishResult.HeroesAreWinners;

        CountCombatFinished();

        // See UpdateCombatFinished next
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
        _combatCore.CombatFinished += CombatCore_CombatFinished;
        _combatCore.CombatantUsedMove += CombatCore_CombatantUsedMove;

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

        var intentionFactory =
            new BotCombatActorIntentionFactory(_animationManager, _combatMovementVisualizer, _gameObjects);
        _combatCore.Initialize(
            CombatantFactory.CreateHeroes(_playerCombatantBehaviour),
            CombatantFactory.CreateMonsters(new BotCombatActorBehaviour(intentionFactory)));

        _unitStatePanelController = new UnitStatePanelController(_combatCore,
            _uiContentStorage, _gameObjectContentStorage);
    }

    private void CombatMovementsHandPanel_CombatMovementPicked(object? sender, CombatMovementPickedEventArgs e)
    {
        var intention = new UseCombatMovementIntention(e.CombatMovement, _animationManager, _combatMovementVisualizer,
            _gameObjects);

        _playerCombatantBehaviour.Assign(intention);
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

    private void DrawCombatMovementsPanel(SpriteBatch spriteBatch, Rectangle contentRectangle)
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

        if (!_combatCore.Finished && _combatCore.CurrentCombatant.IsPlayerControlled)
        {
            if (!_animationManager.HasBlockers)
            {
                _maneuversVisualizer.Draw(spriteBatch);

                _maneuversIndicator.Rect =
                    new Rectangle(contentRectangle.Center.X - 100, contentRectangle.Bottom - 105, 200, 25);
                _maneuversIndicator.Draw(spriteBatch);
            }

            DrawCombatMovementsPanel(spriteBatch, contentRectangle);
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

    private CombatantGameObject GetCombatantGameObject(Combatant combatant)
    {
        return _gameObjects.First(x => x.Combatant == combatant);
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

    private void ManeuverVisualizer_ManeuverSelected(object? sender, ManeuverSelectedEventArgs e)
    {
        if (sender is null)
        {
            throw new InvalidOperationException("Can't handle event of non-instance.");
        }

        var maneuverDirection = CalcDirection(_combatCore.CurrentCombatant, e.Coords);

        if (maneuverDirection is not null)
        {
            var maneuverIntention = new ManeverIntention(maneuverDirection.Value);

            _playerCombatantBehaviour.Assign(maneuverIntention);
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
            var completedCombats = new List<CombatSource>
                { _args.CombatSequence.Combats.First() };

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

    private void UpdateCombatHud(GameTime gameTime)
    {
        if (!_combatCore.Finished && _combatCore.CurrentCombatant.IsPlayerControlled)
        {
            if (!_animationManager.HasBlockers)
            {
                _maneuversVisualizer.Update(ResolutionIndependentRenderer);
            }

            if (_combatMovementsHandPanel is not null)
            {
                _combatMovementsHandPanel.Readonly = _animationManager.HasBlockers;
                _combatMovementsHandPanel.Update(gameTime, ResolutionIndependentRenderer);
            }
        }

        _unitStatePanelController?.Update(ResolutionIndependentRenderer);
    }
}