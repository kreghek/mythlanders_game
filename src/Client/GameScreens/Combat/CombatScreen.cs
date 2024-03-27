using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Client.Assets;
using Client.Assets.ActorVisualizationStates.Primitives;
using Client.Assets.Catalogs;
using Client.Assets.CombatMovements;
using Client.Assets.CombatVisualEffects;
using Client.Assets.StoryPointJobs;
using Client.Core;
using Client.Core.Campaigns;
using Client.Engine;
using Client.Engine.PostProcessing;
using Client.GameScreens.Campaign;
using Client.GameScreens.Combat.CombatDebugElements;
using Client.GameScreens.Combat.GameObjects;
using Client.GameScreens.Combat.GameObjects.Background;
using Client.GameScreens.Combat.Tutorial;
using Client.GameScreens.Combat.Ui;
using Client.GameScreens.CommandCenter;
using Client.GameScreens.Common;
using Client.ScreenManagement;

using CombatDicesTeam.Combats;
using CombatDicesTeam.Dices;
using CombatDicesTeam.Engine.Ui;

using Core.Combats.BotBehaviour;
using Core.PropDrop;
using Core.Props;

using GameAssets.Combats;

using GameClient.Engine;
using GameClient.Engine.RectControl;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonoGame;
using MonoGame.Extended.TextureAtlases;

namespace Client.GameScreens.Combat;

internal class CombatScreen : GameScreenWithMenuBase
{
    private const int BACKGROUND_LAYERS_COUNT = 4;

    private const double ROUND_LABEL_LIFETIME_SEC = 10;

    private readonly UpdatableAnimationManager _animationBlockManager;
    private readonly CombatScreenTransitionArguments _args;
    private readonly CameraOperator _cameraOperator;
    private readonly IReadOnlyCollection<IBackgroundObject> _cloudLayerObjects;
    private readonly ParallaxCamera2DAdapter _combatActionCamera;

    private readonly IList<EffectNotification> _combatantEffectNotifications = new List<EffectNotification>();
    private readonly ICombatantPositionProvider _combatantPositionProvider;
    private readonly MythlandersCombatEngine _combatCore;
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
    private readonly GlobeProvider _globeProvider;
    private readonly InteractionDeliveryManager _interactionDeliveryManager;
    private readonly IJobProgressResolver _jobProgressResolver;
    private readonly ICamera2DAdapter _mainCamera;
    private readonly IReadOnlyList<IBackgroundObject> _mainLayerObjects;
    private readonly FieldManeuverIndicatorPanel _maneuversIndicator;
    private readonly FieldManeuversVisualizer _maneuversVisualizer;
    private readonly ManualCombatActorBehaviour _manualCombatantBehaviour;
    private readonly PostEffectCatalog _postEffectCatalog;
    private readonly PostEffectManager _postEffectManager;
    private readonly RenderTarget2D _renderTarget;
    private readonly ShadeService _shadeService;

    private readonly TargetMarkersVisualizer _targetMarkers;
    private readonly IUiContentStorage _uiContentStorage;
    private readonly VisualEffectManager _visualEffectManager;
    private Texture2D _bloodParticleTexture = null!;
    private SoundEffect _bloodSound;

    private bool _bossWasDefeat;

    private CombatantQueuePanel? _combatantQueuePanel;
    private double _combatFinishedDelayCounter;

    private bool? _combatFinishedVictory;
    private CombatMovementsHandPanel? _combatMovementsHandPanel;

    private bool _combatResultModalShown;

    private double? _combatRoundCounter;

    private bool _finalBossWasDefeat;
    private SoundEffect _shieldBreakingSound = null!;
    private TextureRegion2D _shieldParticleTexture = null!;
    private SoundEffect _shieldSound = null!;

    public CombatScreen(MythlandersGame game, CombatScreenTransitionArguments args) : base(game)
    {
        _args = args;
        var soundtrackManager = Game.Services.GetService<SoundtrackManager>();

        _globeProvider = game.Services.GetService<GlobeProvider>();
        _mainCamera = Game.Services.GetService<ICamera2DAdapter>();

        _globe = _globeProvider.Globe;

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

        var backgroundObjectFactory = bgofSelector.GetBackgroundObjectFactory(_args.Location);

        _cloudLayerObjects = backgroundObjectFactory.CreateCloudLayerObjects();
        _foregroundLayerObjects = backgroundObjectFactory.CreateForegroundLayerObjects();
        _farLayerObjects = backgroundObjectFactory.CreateFarLayerObjects();
        _mainLayerObjects = backgroundObjectFactory.CreateMainLayerObjects();

        _gameSettings = game.Services.GetService<GameSettings>();

        _combatantPositionProvider = new CombatantPositionProvider(MythlandersConstants.CombatFieldSize.X);

        _jobProgressResolver = new JobProgressResolver();

        _manualCombatantBehaviour = new ManualCombatActorBehaviour();

        _combatCore = CreateCombat();
        _combatDataBehaviourProvider = new CombatActorBehaviourDataProvider(_combatCore);

        soundtrackManager.PlayCombatTrack(ExtractCultureFromLocation(args.Location));

        _maneuversVisualizer =
            new FieldManeuversVisualizer(_combatantPositionProvider, new ManeuverContext(_combatCore),
                _uiContentStorage.GetTitlesFont());

        _maneuversIndicator = new FieldManeuverIndicatorPanel(UiThemeManager.UiContentStorage.GetTitlesFont(),
            new ManeuverContext(_combatCore));

        _targetMarkers = new TargetMarkersVisualizer();

        _dropResolver = game.Services.GetRequiredService<IDropResolver>();

        _shadeService = new ShadeService();

        var locationTheme = LocationHelper.GetLocationTheme(args.Location);

        var backgroundTextures = _gameObjectContentStorage.GetCombatBackgrounds(locationTheme);

        var parallaxLayerSpeeds = new[]
        {
            new Vector2(-0.0025f, -0.00025f), // horizon
            new Vector2(-0.005f, -0.0005f), // far layer
            new Vector2(-0.01f, -0.001f), // closest layer
            new Vector2(-0.05f, -0.005f), // main layer
            new Vector2(-0.075f, -0.0075f) // Foreground layer
        };

        var parallaxLayerRectangle = backgroundTextures.First().Bounds;
        var backgroundRectControl = new ParallaxRectControl(
            ResolutionIndependentRenderer.ViewportAdapter.BoundingRectangle,
            parallaxLayerRectangle,
            parallaxLayerSpeeds, new ParallaxViewPointProvider(ResolutionIndependentRenderer));

        var layerCameras = parallaxLayerSpeeds.Select(_ => CreateLayerCamera()).ToArray();

        _combatActionCamera = new ParallaxCamera2DAdapter(
            backgroundRectControl,
            ResolutionIndependentRenderer,
            layerCameras[(int)BackgroundLayerType.Main],
            layerCameras);

        _cameraOperator = new CameraOperator(_combatActionCamera,
            new OverviewCameraOperatorTask(() =>
                backgroundRectControl.GetRects()[(int)BackgroundLayerType.Main].Center.ToVector2() +
                parallaxLayerRectangle.Center.ToVector2()));

        _renderTarget = new RenderTarget2D(Game.GraphicsDevice,
            Game.GraphicsDevice.PresentationParameters.BackBufferWidth,
            Game.GraphicsDevice.PresentationParameters.BackBufferHeight);

        _postEffectCatalog = new PostEffectCatalog();
        _postEffectManager = new PostEffectManager(_postEffectCatalog);
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

        DrawMainGameScene(spriteBatch);

        DrawHud(spriteBatch, contentRectangle);
    }

    protected override void InitializeContent()
    {
        _postEffectCatalog.Load(Game.Content);

        _bloodParticleTexture = new Texture2D(Game.GraphicsDevice, 1, 1);
        _bloodParticleTexture.SetData(new[] { Color.Red });

        var particleTexture = Game.Content.Load<Texture2D>("Sprites/GameObjects/SfxObjects/Particles");
        _shieldParticleTexture = new TextureRegion2D(particleTexture, new Rectangle(0, 32 * 3, 32, 32));

        _bloodSound = Game.Content.Load<SoundEffect>("Audio/Sfx/Blood");
        _shieldSound = Game.Content.Load<SoundEffect>("Audio/Sfx/" + _gameSettings.ShieldSound);
        _shieldBreakingSound = Game.Content.Load<SoundEffect>("Audio/Sfx/ShieldBreaking");

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

        _combatActionCamera.Update();

        UpdateBackgroundObjects(gameTime);

        UpdateInteractionDeliveriesAndUnregisterDestroyed(gameTime);

        _visualEffectManager.Update(gameTime);

        UpdateCombatants(gameTime);

        if (!_combatCore.StateStrategy
                .CalculateCurrentState(new CombatStateStrategyContext(_combatCore.CurrentCombatants,
                    _combatCore.CurrentRoundNumber)).IsFinalState
            && _combatFinishedVictory is null)
        {
            UpdateCombatHud(gameTime);
        }

        if (_combatFinishedVictory is not null)
        {
            UpdateCombatFinished(gameTime);
        }

        _animationBlockManager.Update(gameTime.ElapsedGameTime.TotalSeconds);

        _cameraOperator.Update(gameTime);

        _postEffectManager.Update(gameTime);

        UpdateCombatRoundLabel(gameTime);
    }

    private void AddHitShaking(bool hurt = false)
    {
        IPostEffect postEffect;
        if (!hurt)
        {
            postEffect = new ConstantShakePostEffect(new ShakePower(0.02f));
            _postEffectManager.AddEffect(postEffect);
        }
        else
        {
            postEffect = new HurtPostEffect(new ShakePower(0.02f));
            _postEffectManager.AddEffect(postEffect);
        }

        var postEffectBlocker = new DelayBlocker(new Duration(0.25f));
        _animationBlockManager.RegisterBlocker(postEffectBlocker);
        postEffectBlocker.Released += (_, _) =>
        {
            _postEffectManager.RemoveEffect(postEffect);
        };
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

    private void AssignCombatMovementIntention(CombatMovementInstance combatMovementInstance)
    {
        _targetMarkers.EriseTargets();

        var intention = new UseCombatMovementIntention(
            combatMovementInstance,
            _animationBlockManager,
            _combatMovementVisualizer,
            _gameObjects,
            _interactionDeliveryManager,
            _gameObjectContentStorage,
            _cameraOperator,
            _shadeService,
            _combatantPositionProvider,
            _combatCore.Field,
            _visualEffectManager,
            _postEffectManager);

        _manualCombatantBehaviour.Assign(intention);
    }

    private CombatStepDirection? CalcDirection(ICombatant combatant, FieldCoords targetCoords)
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
        var textPosition = unitGameObject.Graphics.Root.RootNode.Position;
        var font = _uiContentStorage.GetCombatIndicatorFont();

        var passIndicator = new SkipTextIndicator(textPosition, font);

        _visualEffectManager.AddEffect(passIndicator);
    }

    private void CombatCode_CombatantHasBeenAdded(object? sender, CombatantHasBeenAddedEventArgs e)
    {
        var unitCatalog = Game.Services.GetRequiredService<ICombatantGraphicsCatalog>();
        var graphicConfig = unitCatalog.GetGraphics(e.Combatant.ClassSid);

        var combatantSide = e.FieldInfo.FieldSide == _combatCore.Field.HeroSide
            ? CombatantPositionSide.Heroes
            : CombatantPositionSide.Monsters;
        var gameObject =
            new CombatantGameObject(e.Combatant, graphicConfig, e.FieldInfo.CombatantCoords, _combatantPositionProvider,
                _gameObjectContentStorage,
                combatantSide);
        _gameObjects.Add(gameObject);
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

        var corpse =
            combatantGameObject.CreateCorpse(_gameObjectContentStorage, _visualEffectManager, new AudioSettings());
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

        var font = _uiContentStorage.GetCombatIndicatorFont();

        var combatantGameObject = GetCombatantGameObject(e.Combatant);

        var position = combatantGameObject.Graphics.Root.RootNode.Position;

        var nextIndicatorStackIndex = GetIndicatorNextIndex(combatantGameObject) ?? 0;

        combatantGameObject.AnimateDamageImpact();

        if (e.Damage.Amount <= 0 && e.Damage.SourceAmount > 0)
        {
            var blockIndicator = new BlockAnyDamageTextIndicator(position, font, nextIndicatorStackIndex);
            _visualEffectManager.AddEffect(blockIndicator);
        }
        else
        {
            var hitDirection = combatantGameObject.Combatant.IsPlayerControlled
                ? HitDirection.Left
                : HitDirection.Right;

            if (ReferenceEquals(e.StatType, CombatantStatTypes.HitPoints))
            {
                var damageIndicator =
                    new HitPointsChangedTextIndicator(-e.Damage.Amount,
                        StatChangeDirection.Negative,
                        position,
                        font,
                        nextIndicatorStackIndex);

                _visualEffectManager.AddEffect(damageIndicator);

                var bloodEffect = new BloodCombatVisualEffect(combatantGameObject.InteractionPoint,
                    hitDirection,
                    new TextureRegion2D(_bloodParticleTexture));
                _visualEffectManager.AddEffect(bloodEffect);

                _bloodSound.CreateInstance().Play();

                AddHitShaking(combatantGameObject.Combatant.IsPlayerControlled);
            }
            else if (ReferenceEquals(e.StatType, CombatantStatTypes.ShieldPoints))
            {
                var spIndicator =
                    new ShieldPointsChangedTextIndicator(-e.Damage.Amount,
                        StatChangeDirection.Negative,
                        position,
                        font,
                        nextIndicatorStackIndex);

                _visualEffectManager.AddEffect(spIndicator);

                if (e.Combatant.Stats.Single(x => Equals(x.Type, CombatantStatTypes.ShieldPoints)).Value.Current > 0)
                {
                    var shieldEffect = new ShieldCombatVisualEffect(combatantGameObject.InteractionPoint,
                        hitDirection,
                        _shieldParticleTexture,
                        combatantGameObject.CombatantSize);
                    _visualEffectManager.AddEffect(shieldEffect);

                    _shieldSound.CreateInstance().Play();
                }
                else
                {
                    var shieldEffect = new ShieldBreakCombatVisualEffect(combatantGameObject.InteractionPoint,
                        hitDirection,
                        _shieldParticleTexture);
                    _visualEffectManager.AddEffect(shieldEffect);

                    _shieldBreakingSound.CreateInstance().Play();
                }

                AddHitShaking();
            }
        }
    }

    private void CombatCore_CombatantHasChangePosition(object? sender, CombatantHasChangedPositionEventArgs e)
    {
        if (e.Reason == CommonPositionChangeReasons.Maneuver || (e.Reason != CommonPositionChangeReasons.Maneuver &&
                                                                 e.Combatant != _combatCore.CurrentCombatant))
        {
            var newWorldPosition = _combatantPositionProvider.GetPosition(e.NewFieldCoords,
                e.FieldSide == _combatCore.Field.HeroSide
                    ? CombatantPositionSide.Heroes
                    : CombatantPositionSide.Monsters);

            var combatantGameObject = GetCombatantGameObject(e.Combatant);
            combatantGameObject.MoveToFieldCoords(newWorldPosition);
        }
    }

    private void CombatCore_CombatantStartsTurn(object? sender, CombatantTurnStartedEventArgs e)
    {
        var currentCombatantGameObject = GetCombatantGameObject(_combatCore.CurrentCombatant);
        currentCombatantGameObject.Graphics.ShowActiveMarker = true;

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

    private void CombatCore_CombatantStatusHasBeenImposed(object? sender, CombatantStatusEventArgs e)
    {
        _combatantEffectNotifications.Add(
            new EffectNotification(e.Status, e.Combatant, EffectNotificationDirection.Imposed));
        _animationBlockManager.RegisterBlocker(new DelayBlocker(new Duration(2)));
    }

    private const double COMBAT_MOVEMENT_TITLE_DURATION = 1.2;
    private double? _usedCombatMovementCounter;
    private CombatMovementInstance? _usedCombatMovementInstance;
    private bool _usedCombatMovementSide;
    
    private void CombatCore_CombatantUsedMove(object? sender, CombatantHandChangedEventArgs e)
    {
        if (e.Combatant.IsPlayerControlled)
        {
            _combatMovementsHandPanel?.StartMovementBurning(e.HandSlotIndex);
        }

        _usedCombatMovementCounter = COMBAT_MOVEMENT_TITLE_DURATION;
        _usedCombatMovementInstance = e.Move;
        _usedCombatMovementSide = e.Combatant.IsPlayerControlled;
    }

    private void CombatCore_CombatFinished(object? sender, CombatFinishedEventArgs e)
    {
        _combatMovementsHandPanel = null;

        _combatFinishedVictory = e.Result == CommonCombatStates.Victory;

        _globe.ResetCombatScopeJobsProgress();

        CountCombatFinished();

        // See UpdateCombatFinished next
    }

    private void CombatCore_CombatRoundStarted(object? sender, EventArgs e)
    {
        _combatRoundCounter = ROUND_LABEL_LIFETIME_SEC;
    }

    private void CombatMovementsHandPanel_CombatMovementHover(object? sender, CombatMovementPickedEventArgs e)
    {
        var selectorContext =
            new TargetSelectorContext(_combatCore.Field.HeroSide, _combatCore.Field.MonsterSide, _dice,
                _combatCore.CurrentCombatant);
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
        var combatMovementInstance = e.CombatMovement;

        var isMovementAttack = combatMovementInstance.SourceMovement.Tags.HasFlag(CombatMovementTags.Attack);
        var hasTargetsToAttack = _targetMarkers.Targets?.Any(x =>
            x.Target.IsPlayerControlled != _combatCore.CurrentCombatant.IsPlayerControlled) == true;

        if (isMovementAttack && !hasTargetsToAttack)
        {
            AddModal(
                new ConfirmIneffectiveAttackModal(Game.Services.GetService<IUiContentStorage>(),
                    Game.Content.Load<SoundEffect>("Audio/Ui/Alert"),
                    ResolutionIndependentRenderer, () =>
                    {
                        AssignCombatMovementIntention(combatMovementInstance);
                    }), false);
        }
        else
        {
            AssignCombatMovementIntention(combatMovementInstance);
        }
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
                    _args.IsFreeCombat,
                    _args.Location,
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

            _currentCampaign.CompleteCurrentStage();
            _currentCampaign.FailCampaign(_globe, _jobProgressResolver);

            var campaignGenerator = Game.Services.GetService<ICampaignGenerator>();
            var campaigns = campaignGenerator.CreateSet(_globeProvider.Globe);

            ScreenManager.ExecuteTransition(this, ScreenTransition.CommandCenter,
                new CommandCenterScreenTransitionArguments(campaigns));
        }
        else
        {
            Debug.Fail("Unknown combat result.");

            RestoreGroupAfterCombat();

            // Fallback is just showing of new campaign selection.
            _globeProvider.Globe.Update(_dice, _eventCatalog);

            var campaignGenerator = Game.Services.GetService<ICampaignGenerator>();
            var campaigns = campaignGenerator.CreateSet(_globeProvider.Globe);

            _currentCampaign.CompleteCurrentStage();

            ScreenManager.ExecuteTransition(this, ScreenTransition.CommandCenter,
                new CommandCenterScreenTransitionArguments(campaigns));
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

        var jobExecutables = _globe.GetCurrentJobExecutables();

        foreach (var storyPoint in jobExecutables)
        {
            _jobProgressResolver.ApplyProgress(progress, storyPoint);
        }
    }

    private MythlandersCombatEngine CreateCombat()
    {
        return new MythlandersCombatEngine(new CurrentRoundQueueResolver(), _dice);
    }

    private ICamera2DAdapter CreateLayerCamera()
    {
        return new Camera2DAdapter(ResolutionIndependentRenderer.ViewportAdapter)
        {
            Zoom = 1,
            Position = _mainCamera.Position
        };
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
                transformMatrix: _combatActionCamera.LayerCameras[i].GetViewTransformationMatrix());

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

    private void DrawCombatantEffectNotifications(SpriteBatch spriteBatch, Rectangle contentRectangle)
    {
        for (var index = 0; index < _combatantEffectNotifications.Count; index++)
        {
            var notification = _combatantEffectNotifications[index];

            var localizedStatusName = GameObjectHelper.GetLocalized(notification.CombatantEffect.Sid);
            var combatantLocalizedName = GetCombatantLocalizedName(notification);
            var notificationText = string.Format(UiResource.StatusImposedNotificationTextTemplate,
                localizedStatusName, combatantLocalizedName);

            var notificationColor = notification.LifetimeCounter > 0.5
                ? Color.White
                : Color.Lerp(Color.White, Color.Transparent, 1 - (float)notification.LifetimeCounter / 0.5f);

            //TODO Make notification control, add animations, add sound, add blocker to play notification's animation and sound
            // Add status meta-data to store icon, is hidden status, impose(default=true) and dispel notification's display, sound, post-processing effect 
            spriteBatch.DrawString(_uiContentStorage.GetMainFont(),
                notificationText,
                new Vector2(contentRectangle.Center.X, contentRectangle.Top + 50 + index * 15),
                notificationColor);
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
            if (gameObject.IsComplete)
            {
                if (combatSceneContext.CurrentScope is null)
                {
                    // Do not draw corpse while movement scene
                    gameObject.Draw(spriteBatch);
                }
            }
            else
            {
                gameObject.Draw(spriteBatch);
            }
        }

        var list = _gameObjects.OrderBy(x => x.GetZIndex()).ToArray();
        foreach (var gameObject in list)
        {
            if ((combatSceneContext.CurrentScope is not null &&
                 combatSceneContext.CurrentScope.FocusedActors.Contains(gameObject.Animator)) ||
                combatSceneContext.CurrentScope is null)
            {
                gameObject.Graphics.Root.Draw(spriteBatch);
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

    private void DrawCombatantStatuses(Vector2 statsPanelOrigin, ICombatant combatant, SpriteBatch spriteBatch)
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

    private void DrawForegroundLayers(SpriteBatch spriteBatch, IReadOnlyList<Texture2D> backgrounds)
    {
        if (_animationBlockManager.HasBlockers)
        {
            // Do not display foreground layer then combat movement animations are playing.
            return;
        }

        spriteBatch.Begin(
            sortMode: SpriteSortMode.Deferred,
            blendState: BlendState.AlphaBlend,
            samplerState: SamplerState.PointClamp,
            depthStencilState: DepthStencilState.None,
            rasterizerState: RasterizerState.CullNone,
            transformMatrix: _combatActionCamera.LayerCameras[(int)BackgroundLayerType.Closest]
                .GetViewTransformationMatrix());

        spriteBatch.Draw(backgrounds[(int)BackgroundLayerType.Closest], Vector2.Zero, Color.White);

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
            effect.DrawFront(spriteBatch);
        }
    }

    private void DrawGameObjectHud(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin(sortMode: SpriteSortMode.Deferred,
            blendState: BlendState.AlphaBlend,
            samplerState: SamplerState.PointClamp,
            depthStencilState: DepthStencilState.None,
            rasterizerState: RasterizerState.CullNone,
            transformMatrix: _combatActionCamera.LayerCameras[(int)BackgroundLayerType.Main]
                .GetViewTransformationMatrix());

        if (!_combatCore.StateStrategy
                .CalculateCurrentState(new CombatStateStrategyContext(_combatCore.CurrentCombatants,
                    _combatCore.CurrentRoundNumber)).IsFinalState
            && _combatCore.CurrentCombatant.IsPlayerControlled)
        {
            if (!_animationBlockManager.HasBlockers)
            {
                if (!_maneuversVisualizer.IsHidden)
                {
                    // Pressing [ALT] is to show stats and statuses of all combatants.
                    // Hide maneuvers to avoid HUD-mess.
                    if (!Keyboard.GetState().IsKeyDown(Keys.LeftAlt))
                    {
                        _maneuversVisualizer.Draw(spriteBatch);
                    }
                }
            }

            DrawCombatMoveTargets(spriteBatch);

            DrawCombatantsInWorldInfo(spriteBatch);
        }

        spriteBatch.End();
    }

    private void DrawHitPointsStat(SpriteBatch spriteBatch, IStatValue hp, Vector2 barCenter, int arcLength,
        int radiusHp, int sides, int startAngle, int barWidth)
    {
        var barSize = MathHelper.ToRadians(arcLength * (float)hp.GetShare());
        var color = Color.Lerp(Color.Red, Color.Transparent, 0.5f);
        spriteBatch.DrawArc(barCenter, radiusHp, sides, MathHelper.ToRadians(startAngle), barSize, color,
            barWidth);

        var textX = Math.Cos(MathHelper.ToRadians(arcLength * (float)hp.GetShare() + startAngle)) *
            (radiusHp - 2) + barCenter.X;
        var textY = Math.Sin(MathHelper.ToRadians(arcLength * (float)hp.GetShare() + startAngle)) *
            (radiusHp - 2) + barCenter.Y;

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

    private void DrawHud(SpriteBatch spriteBatch, Rectangle contentRectangle)
    {
        if (_gameSettings.IsRecordMode)
        {
            // Do not draw UI for records. Only the amazing picture.
            return;
        }

        DrawGameObjectHud(spriteBatch);

        DrawScreenHud(spriteBatch, contentRectangle);
    }


    private void DrawInteractionDeliveryItems(SpriteBatch spriteBatch)
    {
        foreach (var bullet in _interactionDeliveryManager.GetActiveSnapshot())
        {
            bullet.Draw(spriteBatch);
        }
    }

    private void DrawMainGameObjects(SpriteBatch spriteBatch, ICombatShadeContext combatSceneContext)
    {
        spriteBatch.Begin(sortMode: SpriteSortMode.Deferred,
            blendState: BlendState.AlphaBlend,
            samplerState: SamplerState.PointClamp,
            depthStencilState: DepthStencilState.None,
            rasterizerState: RasterizerState.CullNone,
            transformMatrix: _combatActionCamera.LayerCameras[(int)BackgroundLayerType.Main]
                .GetViewTransformationMatrix());

        DrawBackVisualEffects(spriteBatch);

        DrawInteractionDeliveryItems(spriteBatch);

        DrawCombatants(spriteBatch, combatSceneContext);

        DrawFrontVisualEffects(spriteBatch);

        spriteBatch.End();
    }

    private void DrawMainGameScene(SpriteBatch spriteBatch)
    {
        var locationTheme = LocationHelper.GetLocationTheme(_args.Location);

        var backgrounds = _gameObjectContentStorage.GetCombatBackgrounds(locationTheme);

        var combatSceneContext = GetSceneContext();

        Game.GraphicsDevice.SetRenderTarget(_renderTarget);

        DrawBackgroundLayers(spriteBatch, backgrounds, combatSceneContext);

        DrawMainGameObjects(spriteBatch, combatSceneContext);

        DrawForegroundLayers(spriteBatch, backgrounds);

        Game.GraphicsDevice.SetRenderTarget(null);

        spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

        _postEffectManager.Apply();

        spriteBatch.Draw(_renderTarget, Vector2.Zero, Color.White);
        spriteBatch.End();
    }

    private void DrawManeuverIndicator(SpriteBatch spriteBatch, Rectangle contentRectangle)
    {
        const int MANEUVER_INDICATOR_WIDTH = 200;
        const int MANEUVER_INDICATOR_HEIGHT = 25;
        const int COMBAT_MOVEMENT_PANEL_HEIGHT = 80;

        _maneuversIndicator.Rect = new Rectangle(
            contentRectangle.Center.X - MANEUVER_INDICATOR_WIDTH / 2,
            contentRectangle.Bottom - COMBAT_MOVEMENT_PANEL_HEIGHT - MANEUVER_INDICATOR_HEIGHT,
            MANEUVER_INDICATOR_WIDTH,
            MANEUVER_INDICATOR_HEIGHT);
        _maneuversIndicator.Draw(spriteBatch);
    }

    private void DrawScreenHud(SpriteBatch spriteBatch, Rectangle contentRectangle)
    {
        spriteBatch.Begin(sortMode: SpriteSortMode.Deferred,
            blendState: BlendState.AlphaBlend,
            samplerState: SamplerState.PointClamp,
            depthStencilState: DepthStencilState.None,
            rasterizerState: RasterizerState.CullNone,
            transformMatrix: _mainCamera.GetViewTransformationMatrix());
        
        if (!_combatCore.StateStrategy
                .CalculateCurrentState(new CombatStateStrategyContext(_combatCore.CurrentCombatants,
                    _combatCore.CurrentRoundNumber)).IsFinalState
            && _combatCore.CurrentCombatant.IsPlayerControlled)
        {
            if (!_animationBlockManager.HasBlockers)
            {
                DrawCombatantQueue(spriteBatch, contentRectangle);

                if (!_maneuversVisualizer.IsHidden)
                {
                    if (!_maneuversIndicator.IsHidden)
                    {
                        // ALT to show stats and statuses
                        // Hide maneuvers to avoid HUD-mess
                        if (!Keyboard.GetState().IsKeyDown(Keys.LeftAlt))
                        {
                            DrawManeuverIndicator(spriteBatch, contentRectangle);
                        }
                    }
                }

                //DrawCombatSequenceProgress(spriteBatch);

                DrawCombatMovementsPanel(spriteBatch, contentRectangle);
            }
        }

        DrawCombatantEffectNotifications(spriteBatch, contentRectangle);
        
        DrawUsedCombatMovementTitle(spriteBatch, contentRectangle);

        if (_combatRoundCounter is not null)
        {
            var startX = contentRectangle.Right;
            var endX = contentRectangle.Left;

            var a = 1 - (_combatRoundCounter.Value / ROUND_LABEL_LIFETIME_SEC);
            var t = (float)Math.Sin(a * Math.PI);
            var x = MathHelper.Lerp(startX, endX, t * 0.5f);
            var roundPosition = new Vector2(x, contentRectangle.Top + 20);

            spriteBatch.DrawString(_uiContentStorage.GetTitlesFont(),
                $"-== Round {_combatCore.CurrentRoundNumber}==-",
                roundPosition, MythlandersColors.MainSciFi);
        }

        spriteBatch.End();
    }

    
    
    private void DrawUsedCombatMovementTitle(SpriteBatch spriteBatch, Rectangle contentRectangle)
    {
        if (_usedCombatMovementCounter is null || _usedCombatMovementInstance is null)
        {
            return;
        }
        
        var titlesFont = _uiContentStorage.GetTitlesFont();

        var sourceMovementTitle = GameObjectHelper.GetLocalized(_usedCombatMovementInstance.SourceMovement.Sid);
        var size = titlesFont.MeasureString(sourceMovementTitle);

        var t = 1 - _usedCombatMovementCounter.Value / COMBAT_MOVEMENT_TITLE_DURATION;

        var position = _usedCombatMovementSide ? new Vector2(contentRectangle.Left + ControlBase.CONTENT_MARGIN,
            contentRectangle.Top + ControlBase.CONTENT_MARGIN):
            new Vector2(contentRectangle.Right - ControlBase.CONTENT_MARGIN - size.X,
                contentRectangle.Top + ControlBase.CONTENT_MARGIN);

        var color = Color.Lerp(Color.Transparent, MythlandersColors.MainSciFi, (float)Math.Cos(t * Math.PI * 0.5));
        
        spriteBatch.DrawString(titlesFont, sourceMovementTitle, position, color);
    }

    private void DrawShieldPointsBar(SpriteBatch spriteBatch, IStatValue sp, Vector2 barCenter,
        int arcLength, int sides, int radiusSp, int startAngle, int barWidth)
    {
        var barSize = MathHelper.ToRadians(arcLength * (float)sp.GetShare());
        var color = Color.Lerp(Color.Blue, Color.Transparent, 0.5f);
        spriteBatch.DrawArc(barCenter, radiusSp, sides, MathHelper.ToRadians(startAngle), barSize, color,
            barWidth);

        var textX = Math.Cos(MathHelper.ToRadians(arcLength * (float)sp.GetShare() + startAngle)) *
            radiusSp + barCenter.X;
        var textY = Math.Sin(MathHelper.ToRadians(arcLength * (float)sp.GetShare() + startAngle)) *
            radiusSp + barCenter.Y;

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

    private void DrawStats(Vector2 statsPanelOrigin, ICombatant combatant, SpriteBatch spriteBatch)
    {
        const int SIDES = 32;
        const int START_ANGLE = 180 + 30;
        const int ARC_LENGTH = 180 - 60;
        const int BAR_WIDTH = 3;
        const int RADIUS_SP = 32;
        const int RADIUS_HP = 32 - (BAR_WIDTH - 1);

        var hp = combatant.Stats.Single(x => ReferenceEquals(x.Type, CombatantStatTypes.HitPoints)).Value;
        if (hp.Current > 0)
        {
            DrawHitPointsStat(spriteBatch, hp, statsPanelOrigin, ARC_LENGTH, RADIUS_HP, SIDES, START_ANGLE, BAR_WIDTH);
        }

        var sp = combatant.Stats.Single(x => ReferenceEquals(x.Type, CombatantStatTypes.ShieldPoints)).Value;
        if (sp.Current > 0)
        {
            DrawShieldPointsBar(spriteBatch, sp, statsPanelOrigin, ARC_LENGTH, SIDES, RADIUS_SP + 2, START_ANGLE,
                BAR_WIDTH);
        }
    }

    private void DropSelection(ICombatant combatant)
    {
        var oldCombatUnitGameObject = GetCombatantGameObject(combatant);
        oldCombatUnitGameObject.Graphics.ShowActiveMarker = false;
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


    private static string FirstLetterUppercase(string str)
    {
        return str.Length switch
        {
            0 => string.Empty,
            1 => char.ToUpper(str[0]).ToString(),
            _ => char.ToUpper(str[0]) + str[1..]
        };
    }

    private CombatantGameObject GetCombatantGameObject(ICombatant combatant)
    {
        return _gameObjects.First(x => x.Combatant == combatant);
    }

    private CombatantGameObject? GetCombatantGameObjectOrDefault(ICombatant combatant)
    {
        return _gameObjects.FirstOrDefault(x => x.Combatant == combatant);
    }

    private static string GetCombatantLocalizedName(EffectNotification notification)
    {
        var combatantClassSid = FirstLetterUppercase(notification.Combatant.ClassSid);

        return GameObjectHelper.GetLocalized(combatantClassSid);
    }

    private int? GetIndicatorNextIndex(CombatantGameObject unitGameObject)
    {
        var indicators = _visualEffectManager.Effects.OfType<TextIndicatorBase>();
        var currentIndex = indicators.Count();
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
        _combatCore.CombatantStatusHasBeenImposed += CombatCore_CombatantStatusHasBeenImposed;
        _combatCore.CombatantInterrupted += Combat_CombatantInterrupted;
        _combatCore.CombatRoundStarted += CombatCore_CombatRoundStarted;

        _combatMovementsHandPanel = new CombatMovementsHandPanel(
            Game.Content.Load<Texture2D>("Sprites/Ui/SmallVerticalButtonIcons_White"),
            _uiContentStorage,
            _combatMovementVisualizer);
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
                _shadeService,
                _combatantPositionProvider,
                _combatCore.Field,
                _visualEffectManager,
                _postEffectManager
            );
        _combatCore.Initialize(
            CombatantFactory.CreateHeroes(_manualCombatantBehaviour, _currentCampaign),
            CombatantFactory.CreateMonsters(new BotCombatActorBehaviour(intentionFactory),
                _args.CombatSequence.Combats.First().Monsters));

        _combatantQueuePanel = new CombatantQueuePanel(_combatCore,
            _uiContentStorage,
            new CombatantThumbnailProvider(Game.Content, Game.Services.GetRequiredService<ICombatantGraphicsCatalog>()),
            Game.Services.GetRequiredService<ICombatMovementVisualizationProvider>());
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
            var maneuverIntention = new ManeuverIntention(maneuverDirection.Value);

            _manualCombatantBehaviour.Assign(maneuverIntention);

            //var newWorldPosition = _combatantPositionProvider.GetPosition(e.Coords, CombatantPositionSide.Heroes);

            //var combatantGameObject = GetCombatantGameObject(_combatCore.CurrentCombatant);
            //combatantGameObject.MoveToFieldCoords(newWorldPosition);
        }
    }

    private void RestoreGroupAfterCombat()
    {
        foreach (var hero in _currentCampaign.Heroes)
        {
            // Do nothing
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
        if (!_combatCore.StateStrategy
                .CalculateCurrentState(new CombatStateStrategyContext(_combatCore.CurrentCombatants,
                    _combatCore.CurrentRoundNumber)).IsFinalState
            && _combatCore.CurrentCombatant.IsPlayerControlled)
        {
            if (!_animationBlockManager.HasBlockers)
            {
                if (!_maneuversVisualizer.IsHidden)
                {
                    // Pressing [ALT] is to show stats and statuses of all combatants.
                    // Hide maneuvers to avoid HUD-mess.
                    if (!Keyboard.GetState().IsKeyDown(Keys.LeftAlt))
                    {
                        _maneuversVisualizer.Update(gameTime,
                            _combatActionCamera.LayerCameras[(int)BackgroundLayerType.Main]);
                    }
                }
            }

            if (_combatMovementsHandPanel is not null)
            {
                _combatMovementsHandPanel.Readonly = _animationBlockManager.HasBlockers;
                _combatMovementsHandPanel.Update(gameTime, ResolutionIndependentRenderer);
            }
        }

        _maneuversIndicator.Update(gameTime);

        _combatantQueuePanel?.Update(ResolutionIndependentRenderer);

        _targetMarkers.Update(gameTime);

        UpdateCombatantEffectNotifications(gameTime);

        UpdateUsedCombatMovement(gameTime);
    }

    private void UpdateUsedCombatMovement(GameTime gameTime)
    {
        if (_usedCombatMovementCounter is not null)
        {
            if (_usedCombatMovementCounter > 0)
            {
                _usedCombatMovementCounter -= gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                _usedCombatMovementCounter = null;
                _usedCombatMovementInstance = null;
            }
        }
    }

    private void UpdateCombatRoundLabel(GameTime gameTime)
    {
        if (_combatRoundCounter is null)
        {
            return;
        }

        _combatRoundCounter -= gameTime.ElapsedGameTime.TotalSeconds;

        if (_combatRoundCounter <= 0)
        {
            _combatRoundCounter = null;
        }
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

        public EffectNotification(ICombatantStatus combatantEffect, ICombatant combatant,
            EffectNotificationDirection direction)
        {
            _counter = _notificationDuration.ToTimeSpan().TotalSeconds;
            CombatantEffect = combatantEffect;
            Combatant = combatant;
            Direction = direction;
        }

        public ICombatant Combatant { get; }

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