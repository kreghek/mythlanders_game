using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Engine;

namespace Rpg.Client.GameScreens.CharacterDetails
{
    internal abstract class PanelBase : ControlBase
    {
        private readonly SpriteFont _titleFont;

        protected PanelBase(Texture2D texture, SpriteFont titleFont) : base(texture)
        {
            _titleFont = titleFont;
        }

        protected abstract string TitleResourceId { get; }

        protected override void DrawContent(SpriteBatch spriteBatch, Rectangle contentRect, Color contentColor)
        {
            var title = UiResource.ResourceManager.GetString(TitleResourceId);
            if (title is not null)
            {
                var size = _titleFont.MeasureString(title);
                spriteBatch.DrawString(_titleFont, title, new Vector2(contentRect.Center.X - size.X / 2, contentRect.Center.Y), Color.Wheat);
            }

            var contentRectInner = new Rectangle(contentRect.Left + 5, contentRect.Top + 5, contentRect.Width - 5 * 2,
                contentRect.Height - 5 * 2);

            DrawPanelContent(spriteBatch, contentRectInner);
        }

        protected abstract void DrawPanelContent(SpriteBatch spriteBatch, Rectangle contentRect);
    }
}