using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Rpg.Client.Engine
{
    internal sealed class EntityIconButton<T> : EntityButtonBase<T>
    {
        private readonly Texture2D _icon;
        private readonly Rectangle? _iconRect;

        public EntityIconButton(Texture2D texture, IconData iconData, T entity) : base(texture, entity)
        {
            _icon = iconData.Spritesheet;
            _iconRect = iconData.SourceRect;
        }

        protected override void DrawContent(SpriteBatch spriteBatch, Rectangle contentRect, Color color)
        {
            spriteBatch.Draw(_icon, contentRect, _iconRect, color);
        }
    }
}