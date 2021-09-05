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
        private readonly GraphicsDeviceManager _graphics;
        private ScreenManager? _screenManager;

        private SpriteBatch? _spriteBatch;

        public EwarGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _screenManager.Draw(gameTime, _spriteBatch);

            base.Draw(gameTime);
        }

        protected override void Initialize()
        {
            _screenManager = new ScreenManager(this);
            Services.AddService<IScreenManager>(_screenManager);

            Globe globe = CreateGlobe();

            Services.AddService(globe);

            var uiContentStorage = new UiContentStorage();
            Services.AddService<IUiContentStorage>(uiContentStorage);

            var gameObjectsContentStorage = new GameObjectContentStorage();
            Services.AddService(gameObjectsContentStorage);

            Services.AddService<IDice>(new LinearDice());

            Services.AddService(new AnimationManager());

            Services.AddService(_graphics);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            var gameObjectContentStorage = Services.GetService<GameObjectContentStorage>();
            gameObjectContentStorage.LoadContent(Content);

            var uiContentStorage = Services.GetService<IUiContentStorage>();
            uiContentStorage.LoadContent(Content);
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
        }

        private static Globe CreateGlobe()
        {
            return new Globe
            {
                Player = new Player
                {
                    Group = new Group
                    {
                        Units = new[]
                        {
                            new Unit(UnitSchemeCatalog.SlavicHero, 1)
                            {
                                IsPlayerControlled = true
                            },
                            new Unit(UnitSchemeCatalog.HerbalistHero, 1)
                            {
                                IsPlayerControlled = true
                            }
                        }
                    }
                }
            };
        }
    }
}