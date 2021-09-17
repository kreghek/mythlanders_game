using System.Collections.Generic;

using Rpg.Client.Core.Effects;

namespace Rpg.Client.Core.Skills
{
    internal class HealSkill : SkillBase
    {
        public override IEnumerable<EffectRule> Rules { get; } = new List<EffectRule>
        {
            new EffectRule
            {
                Direction = SkillDirection.Target,
                EffectCreator = new EffectCreator(u =>
                {
                    var res = new HealEffect
                    {
                        Power = u.Unit.Power,
                        PowerMultiplier = 1,
                        ValueRange = 1
                    };

                    return res;
                })
            }
        };

        public override string Sid => "Heal";
        public override SkillTargetType TargetType => SkillTargetType.Friendly;
        public override SkillType Type => SkillType.Telekinetic;
    }
}