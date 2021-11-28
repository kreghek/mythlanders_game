using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Engine;

namespace Rpg.Client.GameScreens.Combat.Ui
{
    internal sealed class CombatSkillButton : ButtonBase
    {
        private readonly Texture2D _icon;
        private readonly Rectangle? _iconRect;
        private readonly Texture2D _texture;
        private readonly CombatSkillCard _card;
        private readonly ISkillPanelState _skillPanelState;

        public CombatSkillButton(Texture2D texture, IconData iconData, Rectangle rect, CombatSkillCard card, ISkillPanelState skillPanelState) : base(texture, rect)
        {
            _icon = iconData.Spritesheet;
            _iconRect = iconData.SourceRect;
            _texture = texture;
            _card = card;
            _skillPanelState = skillPanelState;
        }

        protected override void DrawContent(SpriteBatch spriteBatch, Rectangle contentRect, Color color)
        {
            spriteBatch.Draw(_icon, contentRect, _iconRect, color);

            if (!IsEnabled)
            {
                spriteBatch.Draw(_texture, contentRect, _iconRect, Color.Lerp(color, Color.Red, 0.5f));
            }
        }

        protected override Color CalculateColor()
        {
            if (_card == _skillPanelState.SelectedCard)
            {
                return Color.Lerp(Color.White, Color.Cyan, _counter);
            }

            return base.CalculateColor();
        }

        protected override void UpdateContent()
        {
            base.UpdateContent();

            _counter += 0.005f;
            if (_counter > 0.5f)
            {
                _counter = 0.0f;
            }
        }

        private float _counter;
    }
}