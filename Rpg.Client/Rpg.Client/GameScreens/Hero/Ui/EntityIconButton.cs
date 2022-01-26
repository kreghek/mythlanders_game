using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Engine;

namespace Rpg.Client.GameScreens.Hero.Ui
{
    internal sealed class EntityIconButton<T>: ButtonBase
    {
        private readonly Texture2D _icon;
        private readonly Rectangle? _iconRect;
        private readonly T _entity;

        public EntityIconButton(Texture2D texture, IconData iconData, T entity) : base(texture, Rectangle.Empty)
        {
            _entity = entity;
            _icon = iconData.Spritesheet;
            _iconRect = iconData.SourceRect;
        }

        protected override void DrawContent(SpriteBatch spriteBatch, Rectangle contentRect, Color color)
        {
            spriteBatch.Draw(_icon, contentRect, _iconRect, color);
        }

        public T Entity => _entity;
    }
}