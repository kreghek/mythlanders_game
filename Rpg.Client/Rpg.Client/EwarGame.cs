using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Engine;
using Rpg.Client.GameComponents;
using Rpg.Client.Models;
using Rpg.Client.Models.Title;
using Rpg.Client.Screens;

namespace Rpg.Client
{
    public class EwarGame : Game
    {
        private readonly GameObjectContentStorage _gameObjectContentStorage;

        private readonly IScreenManager _screenManager;

        private readonly IUiContentStorage _uiContentStorage;

        private SpriteBatch? _spriteBatch;

        public EwarGame(IUiContentStorage uiContentStorage, GameObjectContentStorage gameObjectContentStorage,
            IScreenManager screenManager)
        {
            _uiContentStorage = uiContentStorage;
            _gameObjectContentStorage = gameObjectContentStorage;
            _screenManager = screenManager;

            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            RegisterMonogameServices();
        }

        public GraphicsDeviceManager GraphicsDeviceManager { get; private set; }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _screenManager.Draw(gameTime, _spriteBatch);

            base.Draw(gameTime);
        }

        protected override void LoadContent()
        {
            AddDevelopmentComponents(_spriteBatch, _uiContentStorage);

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _gameObjectContentStorage.LoadContent(Content);
            _uiContentStorage.LoadContent(Content);
            _gameObjectContentStorage.LoadContent(Content);
            _uiContentStorage.LoadContent(Content);
        }

        protected override void Update(GameTime gameTime)
        {
            if (_screenManager.ActiveScreen is null)
            {
                var startScreen = new TitleScreen(this);
                _screenManager.ActiveScreen = startScreen;
            }

            _screenManager.Update(gameTime);

            base.Update(gameTime);

            if (_screenManager.ActiveScreen is null)
                if (ScreenManager is not null && ScreenManager.ActiveScreen is null)
                {
                    var startScreen = Services.GetService<TitleScreen>();
                    _screenManager.ActiveScreen = startScreen;
                    ScreenManager.ActiveScreen = ScreenManager.StartScreen;
                }

            _screenManager.Update(gameTime);

            base.Update(gameTime);
        }

        private void AddDevelopmentComponents(SpriteBatch spriteBatch, IUiContentStorage uiContentStorage)
        {
            var fpsCounter = new FpsCounter(this, spriteBatch, uiContentStorage.GetMainFont());
            Components.Add(fpsCounter);

            var versionDisplay = new VersionDisplay(this, spriteBatch, uiContentStorage.GetMainFont());
            Components.Add(versionDisplay);
        }

        private void RegisterMonogameServices()
        {
            GraphicsDeviceManager = new GraphicsDeviceManager(this);
            Services.AddService(GraphicsDeviceManager);
        }
    }
}