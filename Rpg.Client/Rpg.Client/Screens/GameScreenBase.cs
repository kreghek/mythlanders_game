using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Engine;

namespace Rpg.Client.Screens
{
    /// <summary>
    /// This is the base class for all game scenes.
    /// </summary>
    internal abstract class GameScreenBase : EwarDrawableComponentBase, IScreen
    {
        public GameScreenBase(EwarGame game)
        {
            Game = game;
            ScreenManager = game.Services.GetService<IScreenManager>();
        }

        public IScreenManager ScreenManager { get; }

        public IScreen? TargetScreen { get; set; }

        ///// <summary>
        ///// Allows the game component draw your content in game screen
        ///// </summary>
        //public override void DoDraw(SpriteBatch spriteBatch)
        //{
        //    spriteBatch.Begin();
        //    base.Draw(gameTime, spriteBatch);
        //    spriteBatch.End();
        //}
    }
}