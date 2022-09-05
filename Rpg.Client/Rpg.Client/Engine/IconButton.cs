using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Rpg.Client.Engine
{
    internal sealed class IconButton : ButtonBase
    {
        private readonly Texture2D _icon;
        private readonly Rectangle? _iconRect;

        public IconButton(Texture2D icon)
        {
            _icon = icon;
        }

        public IconButton(IconData iconData)
        {
            _icon = iconData.Spritesheet;
            _iconRect = iconData.SourceRect;
        }

        protected override Point CalcTextureOffset() => Point.Zero;

        protected override void DrawContent(SpriteBatch spriteBatch, Rectangle contentRect, Color color)
        {
            spriteBatch.Draw(_icon, contentRect, _iconRect, color);
        }
    }
}