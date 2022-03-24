using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Engine;
using Rpg.Client.GameScreens.Common.SkillEffectDrawers;

namespace Rpg.Client.GameScreens.Combat.Ui
{
    internal class SkillHint : HintBase
    {
        private readonly ISkillEffectDrawer[] _effectDrawers;
        private readonly SpriteFont _font;
        private readonly CombatSkill _skill;
        private readonly CombatUnit _unit;

        public SkillHint(Texture2D texture, SpriteFont font, CombatSkill skill, CombatUnit unit) : base(texture)
        {
            _font = font;
            _skill = skill;
            _unit = unit;

            _effectDrawers = new ISkillEffectDrawer[]
            {
                new DamageEffectDrawer(font),
                new PeriodicDamageEffectDrawer(font),
                new PeriodicSupportDamageEffectDrawer(font),
                new HealEffectDrawer(font),
                new PeriodicHealEffectDrawer(font),
                new StunEffectDrawer(font),
                new IncreaseDamageEffectDrawer(font),
                new DecreaseDamageEffectDrawer(font),
                new LifeDrawEffectDrawer(font)
            };
        }

        protected override void DrawContent(SpriteBatch spriteBatch, Rectangle clientRect, Color contentColor)
        {
            var color = Color.White;

            var combatPower = _skill;

            var skillTitlePosition = clientRect.Location.ToVector2() + new Vector2(5, 15);

            var skillNameText = GameObjectHelper.GetLocalized(combatPower.Skill.Sid);
            if (_skill.IsLocked is not null)
            {
                skillNameText += $" (locked {_skill.IsLocked})";
            }

            spriteBatch.DrawString(_font, skillNameText, skillTitlePosition,
                color);

            var manaCostPosition = skillTitlePosition + new Vector2(0, 10);
            if (combatPower.RedEnergyCost > 0 || combatPower.GreenEnergyCost > 0)
            {
                var redManaCostColor = combatPower.IsAvailable ? color : Color.Red;
                spriteBatch.DrawString(_font,
                    $"R {combatPower.RedEnergyCost} +{combatPower.RedEnergyRegen}",
                    manaCostPosition, redManaCostColor);

                var greenManaCostColor = combatPower.IsAvailable ? color : Color.Red;
                spriteBatch.DrawString(_font,
                    $"G {combatPower.GreenEnergyCost} +{combatPower.GreenEnergyRegen}",
                    manaCostPosition + Vector2.UnitY * 10, greenManaCostColor);
            }

            var ruleBlockPosition = manaCostPosition + new Vector2(0, 20);
            var skillRules = combatPower.Skill.Rules.ToArray();
            for (var ruleIndex = 0; ruleIndex < skillRules.Length; ruleIndex++)
            {
                var rule = skillRules[ruleIndex];
                var effectCreator = rule.EffectCreator;
                var effectToDisplay = effectCreator.Create(_unit, combatPower.Env);

                var rulePosition = ruleBlockPosition + new Vector2(0, 10) * ruleIndex;

                foreach (var effectDrawer in _effectDrawers)
                {
                    if (effectDrawer.Draw(spriteBatch, effectToDisplay, rule, rulePosition))
                    {
                        break;
                    }
                }
            }
        }
    }
}