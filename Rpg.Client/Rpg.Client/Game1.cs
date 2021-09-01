using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Engine;
using Rpg.Client.Models;
using Rpg.Client.Models.Combat;
using Rpg.Client.Models.Map;
using Rpg.Client.Screens;

namespace Rpg.Client
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;

        private SpriteBatch? _spriteBatch;
        private ScreenManager? _screenManager;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _screenManager = new ScreenManager(this);
            Components.Add(_screenManager);

            Globe globe = CreateGlobe();

            Services.AddService(globe);

            var uiContentStorage = new UiContentStorage();
            Services.AddService<IUiContentStorage>(uiContentStorage);

            var gameObjectsContentStorage = new GameObjectContentStorage();
            Services.AddService(gameObjectsContentStorage);

            Services.AddService<IDice>(new LinearDice());

            Services.AddService<AnimationManager>(new AnimationManager());

            base.Initialize();
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
                            new Unit(UnitSchemeCatalog.SlavikHero, 1){
                                IsPlayerControlled = true,
                            },
                            new Unit(UnitSchemeCatalog.SlavikHero, 1){
                                IsPlayerControlled = true,
                            },
                            new Unit(UnitSchemeCatalog.SlavikHero, 1){
                                IsPlayerControlled = true,
                            },
                        }
                    }
                }
            };
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
                var startScreen = new BiomScreen(this, _spriteBatch);
                _screenManager.ActiveScreen = startScreen;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
