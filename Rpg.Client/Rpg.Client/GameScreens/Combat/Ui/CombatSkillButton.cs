using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Engine;

namespace Rpg.Client.GameScreens.Combat.Ui
{
    internal sealed class CombatSkillButton<T> : EntityButtonBase<T> where T : CombatSkill
    {
        private readonly Texture2D _icon;
        private readonly Rectangle? _iconRect;
        private readonly ISkillPanelState _skillPanelState;
        private readonly Texture2D _texture;

        private float _counter;

        public CombatSkillButton(Texture2D texture, IconData iconData, T skill,
            ISkillPanelState skillPanelState) : base(texture, skill)
        {
            _icon = iconData.Spritesheet;
            _iconRect = iconData.SourceRect;
            _texture = texture;
            _skillPanelState = skillPanelState;
        }

        protected override Color CalculateColor()
        {
            if (Entity != _skillPanelState.SelectedSkill)
            {
                return base.CalculateColor();
            }

            return Color.Lerp(Color.White, Color.Cyan, _counter);
        }

        protected override void DrawContent(SpriteBatch spriteBatch, Rectangle contentRect, Color color)
        {
            if (IsEnabled)
            {
                spriteBatch.Draw(_icon, contentRect, _iconRect, color);
                DrawBackground(spriteBatch, color);    
            }
            else
            {
                var disabledColor = Color.Lerp(color, Color.Red, 0.5f + _counter * 0.5f);
                spriteBatch.Draw(_icon, contentRect, _iconRect, disabledColor);
                DrawBackground(spriteBatch, disabledColor);
            }
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
    }
}