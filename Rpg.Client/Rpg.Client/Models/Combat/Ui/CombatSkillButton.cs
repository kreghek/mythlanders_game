using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Engine;

namespace Rpg.Client.Models.Combat.Ui
{
    internal sealed class CombatSkillButton : ButtonBase
    {
        private readonly Texture2D _icon;
        private readonly Rectangle? _iconRect;
        private readonly Texture2D _texture;

        public CombatSkillButton(Texture2D texture, IconData iconData, Rectangle rect) : base(texture, rect)
        {
            _icon = iconData.Spritesheet;
            _iconRect = iconData.SourceRect;
            _texture = texture;
        }

        protected override void DrawContent(SpriteBatch spriteBatch, Rectangle contentRect, Color color)
        {
            spriteBatch.Draw(_icon, contentRect, _iconRect, color);

            if (!IsEnabled)
            {
                spriteBatch.Draw(_texture, contentRect, _iconRect, Color.Red);
            }
        }
    }
}
