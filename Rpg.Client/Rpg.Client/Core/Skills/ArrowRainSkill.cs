using System.Collections.Generic;

using Rpg.Client.Core.Effects;

namespace Rpg.Client.Core.Skills
{
    internal class ArrowRainSkill : SkillBase
    {
        public ArrowRainSkill()
        {
        }

        public ArrowRainSkill(bool costRequired) : base(costRequired)
        {
        }

        public override IEnumerable<EffectRule> Rules { get; } = new[]
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
                        Actor = u
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