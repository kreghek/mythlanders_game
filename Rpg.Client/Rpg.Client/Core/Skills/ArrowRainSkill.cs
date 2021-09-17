using System.Collections.Generic;

using Rpg.Client.Core.Effects;

namespace Rpg.Client.Core.Skills
{
    internal class ArrowRainSkill : SkillBase
    {
        public override IEnumerable<EffectRule> Rules { get; } = new List<EffectRule>
        {
            new EffectRule
            {
                Direction = SkillDirection.AllEnemy,
                EffectCreator = new EffectCreator(u =>
                {
                    var res = new AttackEffect
                    {
                        PowerMultiplier = 0.5f,
                        ValueRange = 1,
                        Power = u.Unit.Power
                    };

                    return res;
                })
            }
        };

        public override string Sid => "Arrow Rain";
        public override SkillTargetType TargetType => SkillTargetType.Enemy;
        public override SkillType Type => SkillType.Range;
    }
}