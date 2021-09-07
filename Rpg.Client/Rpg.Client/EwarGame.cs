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

        private readonly IUiContentStorage _uiContentStorage;

        private SpriteBatch? _spriteBatch;

        public EwarGame(IUiContentStorage uiContentStorage, GameObjectContentStorage gameObjectContentStorage)
        {
            _uiContentStorage = uiContentStorage;
            _gameObjectContentStorage = gameObjectContentStorage;

            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            RegisterMonogameServices();
        }

        public GraphicsDeviceManager GraphicsDeviceManager { get; private set; }

        public IScreenManager? ScreenManager { get; set; }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            ScreenManager?.Draw(gameTime, _spriteBatch);

            base.Draw(gameTime);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _gameObjectContentStorage.LoadContent(Content);
            _uiContentStorage.LoadContent(Content);
            
            AddDevelopmentComponents(_spriteBatch, _uiContentStorage);
        }

        protected override void Update(GameTime gameTime)
        {
            if (ScreenManager is not null && ScreenManager.ActiveScreen is null)
                ScreenManager.ActiveScreen = ScreenManager.StartScreen;
            ScreenManager?.Update(gameTime);

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