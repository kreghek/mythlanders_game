using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Client.Assets;
using Client.Assets.ActorVisualizationStates.Primitives;
using Client.Assets.Catalogs;
using Client.Assets.CombatMovements;
using Client.Assets.StoryPointJobs;
using Client.Core;
using Client.Core.Campaigns;
using Client.Engine;
using Client.GameScreens.Campaign;
using Client.GameScreens.Combat.CombatDebugElements;
using Client.GameScreens.Combat.GameObjects;
using Client.GameScreens.Combat.GameObjects.Background;
using Client.GameScreens.Combat.Tutorial;
using Client.GameScreens.Combat.Ui;
using Client.GameScreens.CommandCenter;
using Client.GameScreens.Common;
using Client.ScreenManagement;

using Core.Combats;
using Core.Combats.BotBehaviour;
using Core.Dices;
using Core.PropDrop;
using Core.Props;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonoGame;

namespace Client.GameScreens.Combat;

internal class CombatScreen : GameScreenWithMenuBase
{
    private const int BACKGROUND_LAYERS_COUNT = 4;

    private readonly UpdatableAnimationManager _animationBlockManager;
    private readonly CombatScreenTransitionArguments _args;
    private readonly CameraOperator _cameraOperator;
    private readonly IReadOnlyCollection<IBackgroundObject> _cloudLayerObjects;
    private readonly ICamera2DAdapter _combatActionCamera;

    private readonly IList<EffectNotification> _combatantEffectNotifications = new List<EffectNotification>();
    private readonly ICombatantPositionProvider _combatantPositionProvider;
    private readonly CombatCore _combatCore;
    private readonly ICombatActorBehaviourDataProvider _combatDataBehaviourProvider;
    private readonly ICombatMovementVisualizationProvider _combatMovementVisualizer;
    private readonly IList<CorpseGameObject> _corpseObjects;
    private readonly HeroCampaign _currentCampaign;
    private readonly IDice _dice;
    private readonly IDropResolver _dropResolver;
    private readonly IEventCatalog _eventCatalog;
    private readonly IReadOnlyList<IBackgroundObject> _farLayerObjects;
    private readonly IReadOnlyList<IBackgroundObject> _foregroundLayerObjects;
    private readonly GameObjectContentStorage _gameObjectContentStorage;
    private readonly IList<CombatantGameObject> _gameObjects;
    private readonly GameSettings _gameSettings;
    private readonly Globe _globe;
    private readonly GlobeNode _globeNode;
    private readonly GlobeProvider _globeProvider;
    private readonly InteractionDeliveryManager _interactionDeliveryManager;
    private readonly IJobProgressResolver _jobProgressResolver;
    private readonly ICamera2DAdapter _mainCamera;
    private readonly IReadOnlyList<IBackgroundObject> _mainLayerObjects;
    private readonly FieldManeuverIndicatorPanel _maneuversIndicator;
    private readonly FieldManeuversVisualizer _maneuversVisualizer;
    private readonly ManualCombatActorBehaviour _manualCombatantBehaviour;
    private readonly ScreenShaker _screenShaker;
    private readonly ShadeService _shadeService;

    private readonly TargetMarkersVisualizer _targetMarkers;
    private readonly IUiContentStorage _uiContentStorage;
    private readonly VisualEffectManager _visualEffectManager;

    private bool _bossWasDefeat;

    private CombatantQueuePanel? _combatantQueuePanel;
    private double _combatFinishedDelayCounter;

    private bool? _combatFinishedVictory;
    private CombatMovementsHandPanel? _combatMovementsHandPanel;

    private bool _combatResultModalShown;

    private bool _finalBossWasDefeat;

    public CombatScreen(TestamentGame game, CombatScreenTransitionArguments args) : base(game)
    {
        _args = args;
        var soundtrackManager = Game.Services.GetService<SoundtrackManager>();

        _globeProvider = game.Services.GetService<GlobeProvider>();
        _mainCamera = Game.Services.GetService<ICamera2DAdapter>();
        _combatActionCamera = new Camera2DAdapter(ResolutionIndependentRenderer.ViewportAdapter)
        {
            Zoom = 1,
            Position = _mainCamera.Position
        };

        _cameraOperator = new CameraOperator(_combatActionCamera, new OverviewCameraOperatorTask(_mainCamera.Position));

        _globe = _globeProvider.Globe;

        _globeNode = args.Location;

        _currentCampaign = args.Campaign;

        _gameObjects = new List<CombatantGameObject>();
        _corpseObjects = new List<CorpseGameObject>();
        _interactionDeliveryManager = new InteractionDeliveryManager();
        _visualEffectManager = new VisualEffectManager();

        _gameObjectContentStorage = game.Services.GetService<GameObjectContentStorage>();
        _uiContentStorage = game.Services.GetService<IUiContentStorage>();
        _animationBlockManager = new UpdatableAnimationManager(new AnimationManager());
        _dice = Game.Services.GetService<IDice>();
        _combatMovementVisualizer = Game.Services.GetRequiredService<ICombatMovementVisualizationProvider>();

        var bgofSelector = Game.Services.GetService<BackgroundObjectFactorySelector>();

        _eventCatalog = game.Services.GetService<IEventCatalog>();

        var backgroundObjectFactory = bgofSelector.GetBackgroundObjectFactory(_globeNode.Sid);

        _cloudLayerObjects = backgroundObjectFactory.CreateCloudLayerObjects();
        _foregroundLayerObjects = backgroundObjectFactory.CreateForegroundLayerObjects();
        _farLayerObjects = backgroundObjectFactory.CreateFarLayerObjects();
        _mainLayerObjects = backgroundObjectFactory.CreateMainLayerObjects();

        _gameSettings = game.Services.GetService<GameSettings>();

        _combatantPositionProvider = new CombatantPositionProvider(ResolutionIndependentRenderer.VirtualBounds.Width);

        _screenShaker = new ScreenShaker();

        _jobProgressResolver = new JobProgressResolver();

        _manualCombatantBehaviour = new ManualCombatActorBehaviour();

        _combatCore = CreateCombat();
        _combatDataBehaviourProvider = new CombatActorBehaviourDataProvider(_combatCore);

        soundtrackManager.PlayCombatTrack(ExtractCultureFromLocation(args.Location.Sid));

        _maneuversVisualizer =
            new FieldManeuversVisualizer(_combatantPositionProvider, new ManeuverContext(_combatCore),
                _uiContentStorage.GetTitlesFont());

        _maneuversIndicator = new FieldManeuverIndicatorPanel(UiThemeManager.UiContentStorage.GetTitlesFont(),
            new ManeuverContext(_combatCore));

        _targetMarkers = new TargetMarkersVisualizer();

        _dropResolver = game.Services.GetRequiredService<IDropResolver>();

        _shadeService = new ShadeService();
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
        InitializeCombat();

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

        UpdateInteractionDeliveriesAndUnregisterDestroyed(gameTime);

        _visualEffectManager.Update(gameTime);

        UpdateCombatants(gameTime);

        if (!_combatCore.Finished && _combatFinishedVictory is null)
        {
            UpdateCombatHud(gameTime);
        }

        _screenShaker.Update(gameTime);

        if (_combatFinishedVictory is not null)
        {
            UpdateCombatFinished(gameTime);
        }

        _animationBlockManager.Update(gameTime.ElapsedGameTime.TotalSeconds);

        _cameraOperator.Update(gameTime);
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

    private static void ApplyCombatReward(IReadOnlyCollection<IProp> xpItems, Player player)
    {
        foreach (var item in xpItems)
        {
            player.Inventory.Add(item);
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
        IReadOnlyCollection<IProp> droppedResources)
    {
        var uiRewards = CreateUiModels(droppedResources);

        return new CombatRewards
        {
            InventoryRewards = uiRewards
        };
    }

    private void Combat_CombatantInterrupted(object? sender, CombatantInterruptedEventArgs e)
    {
        var unitGameObject = GetCombatantGameObject(e.Combatant);
        var textPosition = unitGameObject.Position;
        var font = _uiContentStorage.GetCombatIndicatorFont();

        var passIndicator = new SkipTextIndicator(textPosition, font);

        unitGameObject.AddChild(passIndicator);
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
                _gameObjectContentStorage, _combatActionCamera, _screenShaker, combatantSide);
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

        var combatantGameObject = GetCombatantGameObjectOrDefault(e.Combatant);
        if (combatantGameObject is null)
        {
            return;
        }

        var corpse = combatantGameObject.CreateCorpse();
        _corpseObjects.Add(corpse);

        _gameObjects.Remove(combatantGameObject);
    }

    private void CombatCore_CombatantEffectHasBeenImposed(object? sender, CombatantEffectEventArgs e)
    {
        _combatantEffectNotifications.Add(
            new EffectNotification(e.CombatantEffect, e.Combatant, EffectNotificationDirection.Imposed));
        _animationBlockManager.RegisterBlocker(new DelayBlocker(new Duration(2)));
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

            if (e.StatType == CombatantStatTypes.HitPoints)
            {
                var damageIndicator =
                    new HitPointsChangedTextIndicator(-e.Value,
                        HitPointsChangeDirection.Negative,
                        position,
                        font,
                        nextIndex ?? 0);

                unitGameObject.AddChild(damageIndicator);

                unitGameObject.AnimateWound();
            }
            else if (e.StatType == CombatantStatTypes.ShieldPoints)
            {
                var spIndicator =
                    new ShieldPointsChangedTextIndicator(-e.Value,
                        HitPointsChangeDirection.Negative,
                        position,
                        font,
                        nextIndex ?? 0);

                unitGameObject.AddChild(spIndicator);

                unitGameObject.AnimateShield();
            }
            // TODO Display visual effect of stat damage (resolve, maneuvers, etc).
        }
    }

    private void CombatCore_CombatantHasChangePosition(object? sender, CombatantHasChangedPositionEventArgs e)
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
        var currentCombatantGameObject = GetCombatantGameObject(_combatCore.CurrentCombatant);
        currentCombatantGameObject.IsActive = true;

        if (_combatMovementsHandPanel is not null && _combatCore.CurrentCombatant.IsPlayerControlled)
        {
            _combatMovementsHandPanel.IsEnabled = true;
            _combatMovementsHandPanel.Combatant = e.Combatant;

            _targetMarkers.EriseTargets();
            _maneuversVisualizer.IsHidden = false;
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

    private void CombatMovementsHandPanel_CombatMovementHover(object? sender, CombatMovementPickedEventArgs e)
    {
        var selectorContext =
            new TargetSelectorContext(_combatCore.Field.HeroSide, _combatCore.Field.MonsterSide, _dice);
        var targetMarkerContext = new TargetMarkerContext(_combatCore, _gameObjects.ToArray(), selectorContext);
        _targetMarkers.SetTargets(_combatCore.CurrentCombatant, e.CombatMovement.Effects, targetMarkerContext);

        _maneuversVisualizer.IsHidden = true;
    }

    private void CombatMovementsHandPanel_CombatMovementLeave(object? sender, CombatMovementPickedEventArgs e)
    {
        _targetMarkers.EriseTargets();

        _maneuversVisualizer.IsHidden = false;
    }

    private void CombatMovementsHandPanel_CombatMovementPicked(object? sender, CombatMovementPickedEventArgs e)
    {
        _targetMarkers.EriseTargets();

        var intention = new UseCombatMovementIntention(
            e.CombatMovement,
            _animationBlockManager,
            _combatMovementVisualizer,
            _gameObjects,
            _interactionDeliveryManager,
            _gameObjectContentStorage,
            _cameraOperator,
            _shadeService);

        _manualCombatantBehaviour.Assign(intention);
    }

    private void CombatMovementsHandPanel_WaitPicked(object? sender, EventArgs e)
    {
        _targetMarkers.EriseTargets();

        var intention = new WaitIntention();

        _manualCombatantBehaviour.Assign(intention);
    }

    private void CombatResultModal_Closed(object? sender, EventArgs e)
    {
        _animationBlockManager.DropBlockers();

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
                        ScreenManager.ExecuteTransition(this, ScreenTransition.EndGame,
                            new NullScreenTransitionArguments());

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

            ScreenManager.ExecuteTransition(this, ScreenTransition.CommandCenter,
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

            ScreenManager.ExecuteTransition(this, ScreenTransition.CommandCenter,
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

    private static IReadOnlyCollection<ResourceReward> CreateUiModels(IReadOnlyCollection<IProp> droppedResources)
    {
        var rewardList = new List<ResourceReward>();
        foreach (var resource in droppedResources.OfType<Resource>().ToArray())
        {
            var icon = EquipmentItemType.ExperiencePoints;
            switch (resource.Scheme.Sid)
            {
                case "combat-xp":
                    icon = EquipmentItemType.ExperiencePoints;
                    break;
                case "digital-claws":
                    icon = EquipmentItemType.Warrior;
                    break;
                case "bondages":
                    icon = EquipmentItemType.Warrior;
                    break;
            }

            var reward = new ResourceReward
            {
                Amount = resource.Count,
                Type = icon,
                StartValue = 0
            };

            rewardList.Add(reward);
        }

        return rewardList;
    }


    private void DrawBackgroundLayers(SpriteBatch spriteBatch, IReadOnlyList<Texture2D> backgrounds,
        ICombatShadeContext combatSceneContext)
    {
        var color = combatSceneContext.CurrentScope is null ? Color.White : Color.Lerp(Color.White, Color.Black, 0.75f);

        for (var i = 0; i < BACKGROUND_LAYERS_COUNT; i++)
        {
            spriteBatch.Begin(
                sortMode: SpriteSortMode.Deferred,
                blendState: BlendState.AlphaBlend,
                samplerState: SamplerState.PointClamp,
                depthStencilState: DepthStencilState.None,
                rasterizerState: RasterizerState.CullNone,
                transformMatrix: _combatActionCamera.GetViewTransformationMatrix());

            spriteBatch.Draw(backgrounds[i], Vector2.Zero, color);

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

    private void DrawBackVisualEffects(SpriteBatch spriteBatch)
    {
        foreach (var effect in _visualEffectManager.Effects)
        {
            effect.DrawBack(spriteBatch);
        }
    }

    private void DrawCombatantInWorldInfo(SpriteBatch spriteBatch, CombatantGameObject combatant)
    {
        DrawStats(combatant.StatsPanelOrigin, combatant.Combatant, spriteBatch);
        DrawCombatantStatuses(combatant.StatsPanelOrigin, combatant.Combatant, spriteBatch);
    }

    private void DrawCombatantQueue(SpriteBatch spriteBatch, Rectangle contentRectangle)
    {
        if (_combatantQueuePanel is not null)
        {
            var contentSize = _combatantQueuePanel.CalcContentSize();

            _combatantQueuePanel.Rect = new Rectangle(
                new Point(contentRectangle.Center.X - contentSize.X / 2, contentRectangle.Top),
                contentSize);
            _combatantQueuePanel.Draw(spriteBatch);
        }
    }

    private void DrawCombatants(SpriteBatch spriteBatch, ICombatShadeContext combatSceneContext)
    {
        var corpseList = _corpseObjects.OrderBy(x => x.GetZIndex()).ToArray();
        foreach (var gameObject in corpseList)
        {
            gameObject.Draw(spriteBatch);
        }

        var list = _gameObjects.OrderBy(x => x.GetZIndex()).ToArray();
        foreach (var gameObject in list)
        {
            if ((combatSceneContext.CurrentScope is not null &&
                 combatSceneContext.CurrentScope.FocusedActors.Contains(gameObject.Animator)) ||
                combatSceneContext.CurrentScope is null)
            {
                gameObject.Draw(spriteBatch);
            }
        }
    }

    private void DrawCombatantsInWorldInfo(SpriteBatch spriteBatch)
    {
        if (_targetMarkers.Targets is null)
        {
            if (!Keyboard.GetState().IsKeyDown(Keys.LeftAlt))
            {
                return;
            }

            foreach (var combatant in _gameObjects)
            {
                if (combatant.Combatant.IsDead)
                {
                    continue;
                }

                DrawCombatantInWorldInfo(spriteBatch: spriteBatch, combatant: combatant);
            }
        }
        else
        {
            foreach (var target in _targetMarkers.Targets)
            {
                if (target.Target.IsDead)
                {
                    continue;
                }

                var combatantGameObject = GetCombatantGameObject(target.Target);
                DrawCombatantInWorldInfo(spriteBatch: spriteBatch, combatant: combatantGameObject);
            }
        }
    }

    private void DrawCombatantStatuses(Vector2 statsPanelOrigin, Combatant combatant, SpriteBatch spriteBatch)
    {
        var orderedCombatantStatuses = combatant.Statuses.OrderBy(x => x.Sid.ToString()).ToArray();
        for (var statusIndex = 0; statusIndex < orderedCombatantStatuses.Length; statusIndex++)
        {
            var combatantStatus = orderedCombatantStatuses[statusIndex];
            const int COMBATANT_SPRITE_SIZE = 32;
            const int STATUS_ICON_SIZE = 16;
            const int STATUS_MARGIN = 2;
            const int STATUS_HEIGHT = STATUS_ICON_SIZE + STATUS_MARGIN;

            var combatantStatusPosition =
                (statsPanelOrigin + new Vector2(COMBATANT_SPRITE_SIZE, statusIndex * STATUS_HEIGHT)).ToPoint();

            var combatantStatusIconRect = new Rectangle(0, 0, STATUS_ICON_SIZE, STATUS_ICON_SIZE);
            var statusIconDestRectangle =
                new Rectangle(combatantStatusPosition, new Point(STATUS_ICON_SIZE, STATUS_ICON_SIZE));
            spriteBatch.Draw(_uiContentStorage.GetEffectIconsTexture(),
                statusIconDestRectangle,
                combatantStatusIconRect, Color.White);

            var localizedStatusName = GameObjectHelper.GetLocalized(combatantStatus.Sid);
            spriteBatch.DrawString(_uiContentStorage.GetMainFont(), localizedStatusName,
                new Vector2(statusIconDestRectangle.Right + STATUS_MARGIN, statusIconDestRectangle.Y),
                Color.Aqua);
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

    private void DrawCombatMoveTargets(SpriteBatch spriteBatch)
    {
        _targetMarkers.Draw(spriteBatch);
    }

    private void DrawForegroundLayers(SpriteBatch spriteBatch, Texture2D[] backgrounds)
    {
        spriteBatch.Begin(
            sortMode: SpriteSortMode.Deferred,
            blendState: BlendState.AlphaBlend,
            samplerState: SamplerState.PointClamp,
            depthStencilState: DepthStencilState.None,
            rasterizerState: RasterizerState.CullNone,
            transformMatrix: _combatActionCamera.GetViewTransformationMatrix());

        spriteBatch.Draw(backgrounds[4], Vector2.Zero, Color.White);

        foreach (var obj in _foregroundLayerObjects)
        {
            obj.Draw(spriteBatch);
        }

        spriteBatch.End();
    }

    private void DrawFrontVisualEffects(SpriteBatch spriteBatch)
    {
        foreach (var effect in _visualEffectManager.Effects)
        {
            effect.DrawBack(spriteBatch);
        }
    }

    private void DrawGameObjects(SpriteBatch spriteBatch)
    {
        var locationTheme = LocationHelper.GetLocationTheme(_globeNode.Sid);

        var backgrounds = _gameObjectContentStorage.GetCombatBackgrounds(locationTheme);

        var combatSceneContext = GetSceneContext();

        DrawBackgroundLayers(spriteBatch, backgrounds, combatSceneContext);

        spriteBatch.Begin(sortMode: SpriteSortMode.Deferred,
            blendState: BlendState.AlphaBlend,
            samplerState: SamplerState.PointClamp,
            depthStencilState: DepthStencilState.None,
            rasterizerState: RasterizerState.CullNone,
            transformMatrix: _combatActionCamera.GetViewTransformationMatrix());

        DrawBackVisualEffects(spriteBatch);

        DrawInteractionDeliveryItems(spriteBatch);

        DrawCombatants(spriteBatch, combatSceneContext);

        DrawFrontVisualEffects(spriteBatch);

        spriteBatch.End();

        DrawForegroundLayers(spriteBatch, backgrounds);
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
            transformMatrix: _combatActionCamera.GetViewTransformationMatrix());

        if (!_combatCore.Finished && _combatCore.CurrentCombatant.IsPlayerControlled)
        {
            if (!_animationBlockManager.HasBlockers)
            {
                if (!_maneuversVisualizer.IsHidden)
                {
                    // ALT to show stats and statuses
                    // Hide maneuvers to avoid HUD-mess
                    if (!Keyboard.GetState().IsKeyDown(Keys.LeftAlt))
                    {
                        _maneuversVisualizer.Draw(spriteBatch);
                    }

                    if (!_maneuversIndicator.IsHidden)
                    {
                        // ALT to show stats and statuses
                        // Hide maneuvers to avoid HUD-mess
                        if (!Keyboard.GetState().IsKeyDown(Keys.LeftAlt))
                        {
                            _maneuversIndicator.Rect = new Rectangle(contentRectangle.Center.X - 100,
                                contentRectangle.Bottom - 105, 200, 25);
                            _maneuversIndicator.Draw(spriteBatch);
                        }
                    }
                }
            }

            DrawCombatMoveTargets(spriteBatch);

            DrawCombatMovementsPanel(spriteBatch, contentRectangle);

            DrawCombatantsInWorldInfo(spriteBatch);
        }

        spriteBatch.End();

        spriteBatch.Begin(sortMode: SpriteSortMode.Deferred,
            blendState: BlendState.AlphaBlend,
            samplerState: SamplerState.PointClamp,
            depthStencilState: DepthStencilState.None,
            rasterizerState: RasterizerState.CullNone,
            transformMatrix: _mainCamera.GetViewTransformationMatrix());
        try
        {
            DrawCombatantQueue(spriteBatch, contentRectangle);
            //DrawCombatSequenceProgress(spriteBatch);

            for (var index = 0; index < _combatantEffectNotifications.Count; index++)
            {
                var notification = _combatantEffectNotifications[index];

                var localizedStatusName = GameObjectHelper.GetLocalized(notification.CombatantEffect.Sid);
                var combatantLocalizedName = GameObjectHelper.GetLocalized(notification.Combatant.ClassSid);
                var notificationText = string.Format(UiResource.StatusImposedNotificationTextTemplate,
                    localizedStatusName, combatantLocalizedName);

                spriteBatch.DrawString(_uiContentStorage.GetMainFont(),
                    notificationText,
                    new Vector2(contentRectangle.Center.X, contentRectangle.Top + 50 + index * 15),
                    notification.LifetimeCounter > 0.5
                        ? Color.White
                        : Color.Lerp(Color.White, Color.Transparent, 1 - (float)notification.LifetimeCounter / 0.5f));
            }
        }
        catch
        {
            // TODO Fix NRE in the end of the combat with more professional way 
        }

        spriteBatch.End();
    }

    private void DrawInteractionDeliveryItems(SpriteBatch spriteBatch)
    {
        foreach (var bullet in _interactionDeliveryManager.GetActiveSnapshot())
        {
            bullet.Draw(spriteBatch);
        }
    }

    private void DrawStats(Vector2 statsPanelOrigin, Combatant combatant, SpriteBatch spriteBatch)
    {
        const int SIDES = 32;
        const int START_ANGLE = 180 + 30;
        const int ARC_LENGTH = 180 - 60;
        const int BAR_WIDTH = 3;
        const int RADIUS_SP = 32;
        const int RADIUS_HP = 32 - (BAR_WIDTH - 1);

        var barCenter = statsPanelOrigin;

        var hp = combatant.Stats.Single(x => x.Type == CombatantStatTypes.HitPoints).Value;
        if (hp.Current > 0)
        {
            var barSize = MathHelper.ToRadians(ARC_LENGTH * (float)hp.GetShare());
            var color = Color.Lerp(Color.Red, Color.Transparent, 0.5f);
            spriteBatch.DrawArc(barCenter, RADIUS_HP, SIDES, MathHelper.ToRadians(START_ANGLE), barSize, color,
                BAR_WIDTH);

            var textX = Math.Cos(MathHelper.ToRadians(ARC_LENGTH * (float)hp.GetShare() + START_ANGLE)) *
                (RADIUS_HP - 2) + barCenter.X;
            var textY = Math.Sin(MathHelper.ToRadians(ARC_LENGTH * (float)hp.GetShare() + START_ANGLE)) *
                (RADIUS_HP - 2) + barCenter.Y;

            for (var offsetX = -1; offsetX <= 1; offsetX++)
            {
                for (var offsetY = -1; offsetY <= 1; offsetY++)
                {
                    spriteBatch.DrawString(_uiContentStorage.GetMainFont(),
                        hp.Current.ToString(),
                        new Vector2((float)textX + offsetX, (float)textY + offsetY),
                        Color.White);
                }
            }

            spriteBatch.DrawString(_uiContentStorage.GetMainFont(),
                hp.Current.ToString(),
                new Vector2((float)textX, (float)textY),
                Color.Red);
        }

        var sp = combatant.Stats.Single(x => x.Type == CombatantStatTypes.ShieldPoints).Value;
        if (sp.Current > 0)
        {
            var barSize = MathHelper.ToRadians(ARC_LENGTH * (float)sp.GetShare());
            var color = Color.Lerp(Color.Blue, Color.Transparent, 0.5f);
            spriteBatch.DrawArc(barCenter, RADIUS_SP, SIDES, MathHelper.ToRadians(START_ANGLE), barSize, color,
                BAR_WIDTH);

            var textX = Math.Cos(MathHelper.ToRadians(ARC_LENGTH * (float)sp.GetShare() + START_ANGLE)) *
                (RADIUS_SP + 2) + barCenter.X;
            var textY = Math.Sin(MathHelper.ToRadians(ARC_LENGTH * (float)sp.GetShare() + START_ANGLE)) *
                (RADIUS_SP + 2) + barCenter.Y;

            for (var offsetX = -1; offsetX <= 1; offsetX++)
            {
                for (var offsetY = -1; offsetY <= 1; offsetY++)
                {
                    spriteBatch.DrawString(_uiContentStorage.GetMainFont(),
                        sp.Current.ToString(),
                        new Vector2((float)textX + offsetX, (float)textY + offsetY),
                        Color.White);
                }
            }

            spriteBatch.DrawString(_uiContentStorage.GetMainFont(),
                sp.Current.ToString(),
                new Vector2((float)textX, (float)textY),
                Color.Lerp(Color.Blue, Color.Transparent, 0.25f));
        }
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

    private static LocationCulture ExtractCultureFromLocation(ILocationSid locationSid)
    {
        return LocationHelper.GetLocationCulture(locationSid);
    }

    private CombatantGameObject GetCombatantGameObject(Combatant combatant)
    {
        return _gameObjects.First(x => x.Combatant == combatant);
    }

    private CombatantGameObject? GetCombatantGameObjectOrDefault(Combatant combatant)
    {
        return _gameObjects.FirstOrDefault(x => x.Combatant == combatant);
    }

    private static int? GetIndicatorNextIndex(CombatantGameObject unitGameObject)
    {
        var currentIndex = unitGameObject.GetCurrentIndicatorIndex();
        var nextIndex = currentIndex + 1;
        return nextIndex;
    }

    private ICombatShadeContext GetSceneContext()
    {
        return _shadeService.CreateContext();
    }

    private int GetUnbreakableLevel()
    {
        // TODO Like in How wants to be a millionaire?
        // The reaching of some of levels gains unbreakable level.
        return 0;
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

    private void InitializeCombat()
    {
        _combatCore.CombatantHasBeenAdded += CombatCode_CombatantHasBeenAdded;
        _combatCore.CombatantHasBeenDefeated += CombatCode_CombatantHasBeenDefeated;
        _combatCore.CombatantHasBeenDamaged += CombatCore_CombatantHasBeenDamaged;
        _combatCore.CombatantStartsTurn += CombatCore_CombatantStartsTurn;
        _combatCore.CombatantEndsTurn += CombatCore_CombatantEndsTurn;
        _combatCore.CombatantHasChangePosition += CombatCore_CombatantHasChangePosition;
        _combatCore.CombatFinished += CombatCore_CombatFinished;
        _combatCore.CombatantUsedMove += CombatCore_CombatantUsedMove;
        _combatCore.CombatantEffectHasBeenImposed += CombatCore_CombatantEffectHasBeenImposed;
        _combatCore.CombatantInterrupted += Combat_CombatantInterrupted;

        _combatMovementsHandPanel = new CombatMovementsHandPanel(Game, _uiContentStorage, _combatMovementVisualizer);
        _combatMovementsHandPanel.CombatMovementPicked += CombatMovementsHandPanel_CombatMovementPicked;
        _combatMovementsHandPanel.CombatMovementHover += CombatMovementsHandPanel_CombatMovementHover;
        _combatMovementsHandPanel.CombatMovementLeave += CombatMovementsHandPanel_CombatMovementLeave;
        _combatMovementsHandPanel.WaitPicked += CombatMovementsHandPanel_WaitPicked;

        var intentionFactory =
            new BotCombatActorIntentionFactory(
                _animationBlockManager,
                _combatMovementVisualizer,
                _gameObjects,
                _interactionDeliveryManager,
                _gameObjectContentStorage,
                _cameraOperator,
                _shadeService
            );
        _combatCore.Initialize(
            CombatantFactory.CreateHeroes(_manualCombatantBehaviour, _globeProvider.Globe.Player),
            CombatantFactory.CreateMonsters(new BotCombatActorBehaviour(intentionFactory),
                _args.CombatSequence.Combats.First().Monsters));

        _combatantQueuePanel = new CombatantQueuePanel(_combatCore,
            _uiContentStorage,
            new CombatantThumbnailProvider(Game.Content, Game.Services.GetRequiredService<IUnitGraphicsCatalog>()));
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

            _manualCombatantBehaviour.Assign(maneuverIntention);
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
            var isAllCombatSequenceComplete = true;
            if (isAllCombatSequenceComplete)
            {
                // End the combat sequence
                var droppedResources = _dropResolver.Resolve(_args.CombatSequence.Combats[0].Reward.DropTables);

                var rewardItems = CalculateRewardGaining(droppedResources);

                ApplyCombatReward(droppedResources, _globe.Player);
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

    private void UpdateCombatantEffectNotifications(GameTime gameTime)
    {
        foreach (var notification in _combatantEffectNotifications.ToArray())
        {
            notification.Update(gameTime);
            if (notification.LifetimeCounter <= 0)
            {
                _combatantEffectNotifications.Remove(notification);
            }
        }
    }

    private void UpdateCombatants(GameTime gameTime)
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
            if (!_animationBlockManager.HasBlockers)
            {
                _maneuversVisualizer.Update(ResolutionIndependentRenderer);
            }

            if (_combatMovementsHandPanel is not null)
            {
                _combatMovementsHandPanel.Readonly = _animationBlockManager.HasBlockers;
                _combatMovementsHandPanel.Update(gameTime, ResolutionIndependentRenderer);
            }
        }

        _combatantQueuePanel?.Update(ResolutionIndependentRenderer);

        _targetMarkers.Update(gameTime);

        UpdateCombatantEffectNotifications(gameTime);
    }

    private void UpdateInteractionDeliveriesAndUnregisterDestroyed(GameTime gameTime)
    {
        foreach (var bullet in _interactionDeliveryManager.GetActiveSnapshot())
        {
            if (bullet.IsDestroyed)
            {
                _interactionDeliveryManager.Unregister(bullet);
            }
            else
            {
                bullet.Update(gameTime);
            }
        }
    }

    private class EffectNotification
    {
        private readonly TimeOnly _notificationDuration = new(0, 0, 10, 0);
        private double _counter;

        public EffectNotification(ICombatantStatus combatantEffect, Combatant combatant,
            EffectNotificationDirection direction)
        {
            _counter = _notificationDuration.ToTimeSpan().TotalSeconds;
            CombatantEffect = combatantEffect;
            Combatant = combatant;
            Direction = direction;
        }

        public Combatant Combatant { get; }

        public ICombatantStatus CombatantEffect { get; }
        public EffectNotificationDirection Direction { get; }

        public double LifetimeCounter => _counter / _notificationDuration.ToTimeSpan().TotalSeconds;

        public void Update(GameTime gameTime)
        {
            _counter -= gameTime.ElapsedGameTime.TotalSeconds;
        }
    }

    private enum EffectNotificationDirection
    {
        Imposed,
        Dispelled
    }
}