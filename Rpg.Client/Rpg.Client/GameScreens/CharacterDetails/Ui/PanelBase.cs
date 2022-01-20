using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Engine;

namespace Rpg.Client.GameScreens.CharacterDetails
{
    internal abstract class PanelBase : ControlBase
    {
        protected PanelBase(Texture2D texture) : base(texture) { }

        protected override void DrawContent(SpriteBatch spriteBatch, Rectangle contentRect, Color contentColor)
        {
            var contentRectInner = new Rectangle(contentRect.Left + 5, contentRect.Top + 5, contentRect.Width - 5 * 2,
                contentRect.Height - 5 * 2);

            DrawPanelContent(spriteBatch, contentRectInner);
        }

        protected abstract void DrawPanelContent(SpriteBatch spriteBatch, Rectangle contentRect);
    }
}