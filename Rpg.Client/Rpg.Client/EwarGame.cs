using System;
using System.Diagnostics;

using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Engine;
using Rpg.Client.GameComponents;
using Rpg.Client.GameScreens;
using Rpg.Client.GameScreens.Combat.GameObjects.Background;
using Rpg.Client.GameScreens.Intro;
using Rpg.Client.GameScreens.Speech;
using Rpg.Client.GameScreens.Title;
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

            _screenManager = new ScreenManager(this);

            RegisterServices(_screenManager);

            var soundtrackComponent = new SoundtrackManagerComponent(this);
            var soundtrackManager = Services.GetService<SoundtrackManager>();
            soundtrackComponent.Initialize(soundtrackManager);
            Components.Add(soundtrackComponent);

            var uiSoundStorage = Services.GetService<IUiSoundStorage>();
            var uiContentStorage = Services.GetService<IUiContentStorage>();
            UiThemeManager.SoundStorage = uiSoundStorage;
            UiThemeManager.UiContentStorage = uiContentStorage;

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

            var soundtrackManager = Services.GetService<SoundtrackManager>();
            soundtrackManager.Initialize(uiContentStorage);

            var bgofSelector = Services.GetService<BackgroundObjectFactorySelector>();

            bgofSelector.Initialize(gameObjectContentStorage);

#if DEBUG
            if (_gameSettings.Mode == GameMode.Full)
            {
                AddDevelopmentComponents(_spriteBatch, uiContentStorage);
            }
#endif
        }

        protected override void Update(GameTime gameTime)
        {
            if (_screenManager.ActiveScreen is null)
            {
                //var startScreen = new IntroScreen(this);
                _screenManager.ActiveScreen = new SpeechScreen(this); //startScreen;
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

        private void LogGameVersion()
        {
            if (VersionHelper.TryReadVersion(out var version))
            {
                _logger.LogInformation($"Game version info:{Environment.NewLine}{version}");
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

            if (_gameSettings.Mode == GameMode.Full)
            {
                var unitSchemeCatalog = new UnitSchemeCatalog();
                Services.AddService<IUnitSchemeCatalog>(unitSchemeCatalog);

                var biomeGenerator = new BiomeGenerator();
                Services.AddService<IBiomeGenerator>(biomeGenerator);

                var eventCatalog = new EventCatalog(Services.GetService<IUnitSchemeCatalog>());
                Services.AddService<IEventCatalog>(eventCatalog);
            }
            else
            {
                var unitSchemeCatalog = new DemoUnitSchemeCatalog();
                Services.AddService<IUnitSchemeCatalog>(unitSchemeCatalog);

                var biomeGenerator = new DemoBiomeGenerator();
                Services.AddService<IBiomeGenerator>(biomeGenerator);

                var eventCatalog = new DemoEventCatalog(Services.GetService<IUnitSchemeCatalog>());
                Services.AddService<IEventCatalog>(eventCatalog);
            }

            Services.AddService(
                new GlobeProvider(
                    Services.GetService<IDice>(),
                    Services.GetService<IUnitSchemeCatalog>(),
                    Services.GetService<IBiomeGenerator>(),
                    Services.GetService<IEventCatalog>()));

            Services.AddService(new AnimationManager());

            Services.AddService(_graphics);

            var soundtrackManager = new SoundtrackManager();
            Services.AddService(soundtrackManager);

            var bgoFactorySelector = new BackgroundObjectFactorySelector();
            Services.AddService(bgoFactorySelector);

            Services.AddService(new ScreenService());
        }
    }
}