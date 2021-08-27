using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Models.Combat;
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

            var globe = new Globe
            {
                PlayerGroup = new Group
                {
                    Units = new[]
                    {
                        new Unit{

                        }
                    }
                }
            };

            Services.AddService(globe);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (_screenManager.ActiveScreen is null)
            {
                var titleScreen = new CombatScreen(this, _spriteBatch);
                _screenManager.ActiveScreen = titleScreen;
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
