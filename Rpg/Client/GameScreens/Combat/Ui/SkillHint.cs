using System.Linq;

using Client;
using Client.Core;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Engine;
using Rpg.Client.GameScreens.Common.SkillEffectDrawers;

namespace Rpg.Client.GameScreens.Combat.Ui
{
    internal class SkillHint : HintBase
    {
        private readonly ICombat _combat;
        private readonly ISkillEffectDrawer[] _effectDrawers;
        private readonly SpriteFont _font;
        private readonly CombatSkill _skill;
        private readonly ICombatUnit _unit;

        public SkillHint(CombatSkill skill, ICombatUnit unit, ICombat combat)
        {
            _font = UiThemeManager.UiContentStorage.GetMainFont();
            _skill = skill;
            _unit = unit;
            _combat = combat;
            _effectDrawers = EffectDrawersCollector.GetDrawersInAssembly(_font).ToArray();
        }

        protected override Point CalcTextureOffset()
        {
            return Point.Zero;
        }

        protected override void DrawContent(SpriteBatch spriteBatch, Rectangle clientRect, Color contentColor)
        {
            var color = Color.White;

            var skillTitlePosition = clientRect.Location.ToVector2() + new Vector2(5, 15);

            var skillNameText = GameObjectHelper.GetLocalized(_skill.Skill.Sid);

            spriteBatch.DrawString(_font, skillNameText, skillTitlePosition,
                color);

            var manaCostPosition = skillTitlePosition + new Vector2(0, 10);
            if (_skill.EnergyCost > 0)
            {
                var redManaCostColor = _skill.IsAvailable ? color : Color.Red;
                spriteBatch.DrawString(_font,
                    string.Format(UiResource.SkillManaCostTemplate, _skill.EnergyCost),
                    manaCostPosition, redManaCostColor);
            }

            var ruleBlockPosition = manaCostPosition + new Vector2(0, 20);
            var skillRules = _skill.Skill.Rules.ToArray();
            for (var ruleIndex = 0; ruleIndex < skillRules.Length; ruleIndex++)
            {
                var rule = skillRules[ruleIndex];
                var effectCreator = rule.EffectCreator;
                var effectToDisplay = effectCreator.Create(_unit, _combat, _skill.Skill);

                var rulePosition = ruleBlockPosition + new Vector2(0, 10) * ruleIndex;

                foreach (var effectDrawer in _effectDrawers)
                {
                    if (effectDrawer.Draw(spriteBatch, effectToDisplay, rule.Direction, rulePosition))
                    {
                        break;
                    }
                }
            }
        }
    }
}