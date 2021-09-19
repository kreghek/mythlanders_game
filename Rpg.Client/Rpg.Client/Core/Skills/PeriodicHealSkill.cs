using System.Collections.Generic;

using Rpg.Client.Core.Effects;

namespace Rpg.Client.Core.Skills
{
    internal class PeriodicHealSkill : SkillBase
    {
        public override IEnumerable<EffectRule> Rules { get; } = new List<EffectRule>
        {
            new EffectRule
            {
                Direction = SkillDirection.Target,
                EffectCreator = new EffectCreator(u =>
                {
                    var effect = new PeriodicHealEffect
                    {
                        Value = 3,
                        Power = u.Unit.Power,
                        PowerMultiplier = 0.3f,
                        ValueRange = 1
                    };

                    return effect;
                })
            }
        };

        public override string Sid => "Periodic Heal";
        public override SkillTargetType TargetType => SkillTargetType.Friendly;
        public override SkillType Type => SkillType.Range;
    }
}