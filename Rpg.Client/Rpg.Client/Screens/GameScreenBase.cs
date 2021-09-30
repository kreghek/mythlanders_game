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
        private readonly IList<IModalWindow> _modals;

        public GameScreenBase(EwarGame game)
        {
            Game = game;

            ScreenManager = game.Services.GetService<IScreenManager>();

            _modals = new List<IModalWindow>();
        }

        public IScreenManager ScreenManager { get; }

        protected void AddModal(IModalWindow modal, bool isLate)
        {
            _modals.Add(modal);
            if (!isLate)
            {
                modal.Show();
            }
        }

        protected override void DoDraw(SpriteBatch spriteBatch, float zindex)
        {
            base.DoDraw(spriteBatch, zindex);

            DrawContent(spriteBatch);

            DrawModals(spriteBatch);
        }

        protected abstract void DrawContent(SpriteBatch spriteBatch);

        protected abstract void UpdateContent(GameTime gameTime);

        protected void UpdateModals(GameTime gameTime)
        {
            foreach (var modal in _modals)
            {
                if (modal.IsVisible)
                {
                    modal.Update(gameTime);
                    break;
                }
            }
        }

        private void DrawModals(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            foreach (var modal in _modals)
            {
                if (modal.IsVisible)
                {
                    modal.Draw(spriteBatch);
                    break;
                }
            }

            spriteBatch.End();
        }

        public IScreen? TargetScreen { get; set; }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            UpdateContent(gameTime);

            UpdateModals(gameTime);
        }
    }
}