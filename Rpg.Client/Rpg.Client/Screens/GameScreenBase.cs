using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Rpg.Client.Screens
{
    /// <summary>
    /// This is the base class for all game scenes.
    /// </summary>
    public abstract class GameScreenBase : IScreen
    {
        public GameScreenBase(IScreenManager screenManager)
        {
            ScreenManager = screenManager;
        }

        public IScreenManager ScreenManager { get; }

        public IScreen? TargetScreen { get; set; }

        /// <summary>
        /// Allows the game component draw your content in game screen
        /// </summary>
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public abstract void Update(GameTime gameTime);
    }
}