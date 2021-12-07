using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Core.SkillEffects;
using Rpg.Client.Core.Skills;
using Rpg.Client.Engine;

namespace Rpg.Client.GameScreens.Combat.Ui
{
    internal class SkillHint : HintBase
    {
        private readonly SpriteFont _font;
        private readonly CombatSkillCard _skill;
        private readonly CombatUnit _unit;

        public SkillHint(Texture2D texture, SpriteFont font, CombatSkillCard skill, CombatUnit unit) : base(texture)
        {
            _font = font;
            _skill = skill;
            _unit = unit;
        }

        protected override void DrawContent(SpriteBatch spriteBatch, Rectangle clientRect, Color contentColor)
        {
            var color = Color.White;

            var combatPower = _skill;

            var skillTitlePosition = clientRect.Location.ToVector2() + new Vector2(5, 15);

            var skillNameText = GameObjectResources.ResourceManager.GetString(combatPower.Skill.Sid.ToString()) ??
                                $"#Resource-{combatPower.Skill.Sid}";
            spriteBatch.DrawString(_font, skillNameText, skillTitlePosition,
                color);

            var manaCostPosition = skillTitlePosition + new Vector2(0, 10);
            if (combatPower.Skill.ManaCost is not null)
            {
                var manaCostColor = combatPower.IsAvailable ? color : Color.Red;
                spriteBatch.DrawString(_font, $"Cost: {combatPower.Skill.ManaCost}",
                    manaCostPosition, manaCostColor);
            }

            var ruleBlockPosition = manaCostPosition + new Vector2(0, 10);
            var skillRules = combatPower.Skill.Rules.ToArray();
            for (var ruleIndex = 0; ruleIndex < skillRules.Length; ruleIndex++)
            {
                var rule = skillRules[ruleIndex];
                var effectCreator = rule.EffectCreator;
                var effectToDisplay = effectCreator.Create(_unit);

                var rulePosition = ruleBlockPosition + new Vector2(0, 10) * ruleIndex;

                if (effectToDisplay is AttackEffect attackEffect)
                {
                    var damage = attackEffect.CalculateDamage();

                    spriteBatch.DrawString(_font,
                        $"Damage: {damage.Min} - {damage.Max} to {rule.Direction}",
                        rulePosition, color);
                }
                else if (effectToDisplay is HealEffect healEffect)
                {
                    var heal = healEffect.CalculateHeal();
                    spriteBatch.DrawString(_font,
                        $"Heal: {heal.Min} - {heal.Max}", rulePosition, color);
                }
                else if (effectToDisplay is PeriodicHealEffect periodicHealEffect)
                {
                    var heal = periodicHealEffect.CalculateHeal();
                    spriteBatch.DrawString(_font,
                        $"Heal over time: {heal.Min} - {heal.Max}",
                        rulePosition,
                        color);
                }
                else if (effectToDisplay is StunEffect stunEffect)
                {
                    spriteBatch.DrawString(_font,
                        $"Stun: {stunEffect.Duration} turns",
                        rulePosition,
                        color);
                }
                else if (effectToDisplay is IncreaseAttackEffect increaseAttackEffect)
                {
                    spriteBatch.DrawString(_font,
                        $"Power up: {increaseAttackEffect.Duration} turns up to {50}% damage",
                        rulePosition,
                        color);
                }
            }
        }
    }
}