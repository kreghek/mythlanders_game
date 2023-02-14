using System;
using System.Diagnostics;

using Client.Assets.Catalogs;

using Core.Dices;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Rpg.Client.Assets;
using Rpg.Client.Assets.Catalogs;
using Rpg.Client.Core;
using Rpg.Client.Engine;
using Rpg.Client.GameComponents;
using Rpg.Client.GameScreens;
using Rpg.Client.GameScreens.Combat.GameObjects.Background;
using Rpg.Client.ScreenManagement;

namespace Rpg.Client
{
    public class EwarGame : Game
    {
        private readonly GameSettings _gameSettings;
        private readonly GraphicsDeviceManager _graphics;
        private readonly ILogger<EwarGame> _logger;
        private Camera2D _camera;

        private ResolutionIndependentRenderer _resolutionIndependence;
        private ScreenManager? _screenManager;

        private SpriteBatch? _spriteBatch;

        public EwarGame(ILogger<EwarGame> logger, GameMode gameMode)
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _logger = logger;

            _gameSettings = new GameSettings
            {
                Mode = gameMode
            };
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            Debug.Assert(_screenManager is not null);
            Debug.Assert(_spriteBatch is not null);

            _screenManager.Draw(_spriteBatch);

            base.Draw(gameTime);
        }

        protected override void Initialize()
        {
            _logger.LogInformation("Initialization started");

            LogGameVersion();

            Services.AddService(_logger);

            _screenManager = new ScreenManager(this, _gameSettings);

            RegisterServices(_screenManager);

            var soundtrackComponent = new SoundtrackManagerComponent(this);
            var soundtrackManager = Services.GetService<SoundtrackManager>();
            soundtrackComponent.Initialize(soundtrackManager);
            Components.Add(soundtrackComponent);

            _resolutionIndependence = new ResolutionIndependentRenderer(this);
            Services.AddService(_resolutionIndependence);

            _camera = new Camera2D(_resolutionIndependence);
            Services.AddService(_camera);

            Services.AddService(_gameSettings);

#if DEBUG
            const int WIDTH = 848;
            const int HEIGHT = 480;
#else
            var WIDTH = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            var HEIGHT = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
#endif

            InitializeResolutionIndependence(WIDTH, HEIGHT);

#if DEBUG
            _graphics.IsFullScreen = false;
#else
            _graphics.IsFullScreen = true;
#endif

            _graphics.PreferredBackBufferWidth = WIDTH;
            _graphics.PreferredBackBufferHeight = HEIGHT;
            _graphics.ApplyChanges();

            _logger.LogInformation("Initialization complete successfully");

            Services.AddService(_gameSettings.Mode);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            Services.AddService(_spriteBatch);

            var gameObjectContentStorage = Services.GetService<GameObjectContentStorage>();
            gameObjectContentStorage.LoadContent(Content);

            var uiContentStorage = Services.GetService<IUiContentStorage>();
            uiContentStorage.LoadContent(Content);

            var uiSoundStorage = Services.GetService<IUiSoundStorage>();
            uiSoundStorage.LoadContent(Content);

            InitUiThemeManager();

            var soundtrackManager = Services.GetService<SoundtrackManager>();
            soundtrackManager.Initialize(uiContentStorage);

            var bgofSelector = Services.GetService<BackgroundObjectFactorySelector>();

            var backgroundObjectCatalog = new BackgroundObjectCatalog(gameObjectContentStorage);

            var dice = Services.GetService<IDice>();

            bgofSelector.Initialize(gameObjectContentStorage, backgroundObjectCatalog, dice);

#if DEBUG
            if (_gameSettings.Mode == GameMode.Full)
            {
                AddDevelopmentComponents(_spriteBatch, uiContentStorage);
            }
#endif

            Mouse.SetCursor(MouseCursor.FromTexture2D(uiContentStorage.GetCursorsTexture(), 1, 1));
        }

        protected override void Update(GameTime gameTime)
        {
            if (_screenManager is null)
            {
                throw new InvalidOperationException("Screen manager is not initialized before game updates.");
            }

            if (_screenManager.ActiveScreen is null)
            {
                _screenManager.InitStartScreen();
            }

            _screenManager.Update(gameTime);

            base.Update(gameTime);
        }

        private void AddDevelopmentComponents(SpriteBatch spriteBatch, IUiContentStorage uiContentStorage)
        {
            var fpsCounter = new FpsCounter(this, spriteBatch, uiContentStorage.GetMainFont());
            Components.Add(fpsCounter);

            var versionDisplay = new VersionDisplay(this, spriteBatch, uiContentStorage.GetMainFont(), _logger);
            versionDisplay.Initialize();
            Components.Add(versionDisplay);

            var cheatInput = new CheatInput(this, spriteBatch, uiContentStorage.GetMainFont());
            Components.Add(cheatInput);

            var trackNameDisplay = new TrackNameDisplay(this, spriteBatch, uiContentStorage.GetMainFont());
            Components.Add(trackNameDisplay);
        }

        private void InitializeResolutionIndependence(int realScreenWidth, int realScreenHeight)
        {
            _resolutionIndependence.VirtualWidth = 848;
            _resolutionIndependence.VirtualHeight = 480;
            _resolutionIndependence.ScreenWidth = realScreenWidth;
            _resolutionIndependence.ScreenHeight = realScreenHeight;
            _resolutionIndependence.Initialize();

            _camera.Zoom = 1f;
            _camera.Position = _resolutionIndependence.VirtualBounds.Center.ToVector2();
            _camera.RecalculateTransformationMatrices();
        }

        private void InitUiThemeManager()
        {
            var uiSoundStorage = Services.GetService<IUiSoundStorage>();
            var uiContentStorage = Services.GetService<IUiContentStorage>();
            UiThemeManager.SoundStorage = uiSoundStorage;
            UiThemeManager.UiContentStorage = uiContentStorage;
        }

        private void LogGameVersion()
        {
            if (VersionHelper.TryReadVersion(out var version))
            {
                _logger.LogInformation("Game version info:\n{Version}", version);
            }
            else
            {
                _logger.LogError("Can't read game version");
            }
        }

        private void RegisterServices(IScreenManager screenManager)
        {
            Services.AddService(screenManager);

            var uiContentStorage = new UiContentStorage();
            Services.AddService<IUiContentStorage>(uiContentStorage);

            var uiSoundStorage = new UiSoundStorage();
            Services.AddService<IUiSoundStorage>(uiSoundStorage);

            var gameObjectsContentStorage = new GameObjectContentStorage();
            Services.AddService(gameObjectsContentStorage);

            Services.AddService<IDice>(new LinearDice());

            var dialogueResourceProvider = new DialogueResourceProvider();

            var balanceTable = new BalanceTable();
            if (_gameSettings.Mode == GameMode.Full)
            {
                var unitSchemeCatalog = new UnitSchemeCatalog(balanceTable, isDemo: false);
                Services.AddService<IUnitSchemeCatalog>(unitSchemeCatalog);

                var dialogueAftermathCreator = new DialogueOptionAftermathCreator(unitSchemeCatalog);

                var dialogueCatalog = new DialogueCatalog(dialogueResourceProvider, dialogueAftermathCreator);
                Services.AddService<IEventInitializer>(dialogueCatalog);
                Services.AddService<IEventCatalog>(dialogueCatalog);

                var storyPointCatalog = new StoryPointCatalog();
                Services.AddService<IStoryPointInitializer>(storyPointCatalog);
                Services.AddService<IStoryPointCatalog>(storyPointCatalog);
            }
            else
            {
                var unitSchemeCatalog = new UnitSchemeCatalog(balanceTable, isDemo: true);
                Services.AddService<IUnitSchemeCatalog>(unitSchemeCatalog);

                var dialogueAftermathCreator = new DialogueOptionAftermathCreator(unitSchemeCatalog);

                var dialogueCatalog = new DialogueCatalog(dialogueResourceProvider, dialogueAftermathCreator);
                Services.AddService<IEventInitializer>(dialogueCatalog);
                Services.AddService<IEventCatalog>(dialogueCatalog);

                var storyPointCatalog = new DemoStoryPointCatalog();
                Services.AddService<IStoryPointInitializer>(storyPointCatalog);
                Services.AddService<IStoryPointCatalog>(storyPointCatalog);
            }

            var eventInitializer = Services.GetRequiredService<IEventInitializer>();
            eventInitializer.Init();

            Services.AddService(
                new GlobeProvider(
                    Services.GetRequiredService<IDice>(),
                    Services.GetRequiredService<IUnitSchemeCatalog>(),
                    Services.GetRequiredService<IEventCatalog>(),
                    Services.GetRequiredService<IStoryPointInitializer>()));

            var campaignGenerator = new CampaignGenerator(
                Services.GetRequiredService<IUnitSchemeCatalog>(),
                Services.GetRequiredService<GlobeProvider>(),
                Services.GetRequiredService<IEventCatalog>(),
                Services.GetRequiredService<IDice>());
            Services.AddService<ICampaignGenerator>(campaignGenerator);

            Services.AddService(new AnimationManager());

            Services.AddService(_graphics);

            var soundtrackManager = new SoundtrackManager(_gameSettings);
            Services.AddService(soundtrackManager);

            var bgoFactorySelector = new BackgroundObjectFactorySelector();
            Services.AddService(bgoFactorySelector);

            Services.AddService(new ScreenService());
        }
    }
}