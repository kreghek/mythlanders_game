using System;
using System.Collections.Generic;

using Rpg.Client.Core.Modifiers;

namespace Rpg.Client.Core.SkillEffects
{
    internal class DecreaseDamageEffect : ModifiersEffect
    {
        public DecreaseDamageEffect(float multiplier, CombatSkillEfficient efficient)
        {
            var envEfficient = GetEnvModifier(efficient);
            
            Multiplier = multiplier * envEfficient;
            
            Modifiers = new List<ModifierBase>
            {
                new GivenDamageModifier
                {
                    DamageMultiplier = Multiplier
                }
            };
        }
        
        private static float GetEnvModifier(CombatSkillEfficient efficient)
        {
            return efficient switch
            {
                CombatSkillEfficient.Zero => 0,
                CombatSkillEfficient.Low => 0.5f,
                CombatSkillEfficient.Normal => 1f,
                CombatSkillEfficient.High => 2f,
                _ => throw new Exception(),
            };
        }

        public override IEnumerable<ModifierBase> Modifiers { get; }
        public float Multiplier { get; }

        protected override void AfterDispel()
        {
            Target.ChangeState(CombatUnitState.Idle);
        }

        protected override void AfterImpose()
        {
            Target.ChangeState(CombatUnitState.Defense);
        }
    }
}