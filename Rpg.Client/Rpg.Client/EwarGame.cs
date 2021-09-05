using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Engine;
using Rpg.Client.Models;
using Rpg.Client.Models.Title;
using Rpg.Client.Screens;

namespace Rpg.Client
{
    public class EwarGame : Game
    {
        private readonly GameObjectContentStorage _gameObjectContentStorage;

        private SpriteBatch? _spriteBatch;

        private readonly IUiContentStorage _uiContentStorage;

        public EwarGame(IUiContentStorage uiContentStorage, GameObjectContentStorage gameObjectContentStorage)
        {
            _uiContentStorage = uiContentStorage;
            _gameObjectContentStorage = gameObjectContentStorage;

            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            RegisterMonogameServices();
        }

        public IScreenManager? ScreenManager { get; set; }
        public GraphicsDeviceManager GraphicsDeviceManager { get; private set; }

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
        }

        protected override void Update(GameTime gameTime)
        {
            if (ScreenManager is not null && ScreenManager.ActiveScreen is null)
            {
                ScreenManager.ActiveScreen = ScreenManager.StartScreen;
            }
            ScreenManager?.Update(gameTime);

            base.Update(gameTime);
        }
        
        private void RegisterMonogameServices()
        {
            GraphicsDeviceManager = new GraphicsDeviceManager(this);
            Services.AddService(GraphicsDeviceManager);
        }
    }
}