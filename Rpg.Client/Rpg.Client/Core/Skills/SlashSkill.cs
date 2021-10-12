using System.Collections.Generic;

using Rpg.Client.Core.Effects;

namespace Rpg.Client.Core.Skills
{
    internal class SlashSkill : SkillBase
    {
        public SlashSkill()
        {
        }

        public SlashSkill(bool costRequired) : base(costRequired)
        {
        }

        public override IEnumerable<EffectRule> Rules { get; } = new[]
        {
            new EffectRule
            {
                Direction = SkillDirection.Target,
                EffectCreator = new EffectCreator(u =>
                {
                    var res = new AttackEffect
                    {
                        Actor = u,
                        PowerMultiplier = 1,
                        ValueRange = 1
                    };

                    return res;
                })
            }
        };

        public override string Sid => "Slash";
        public override SkillTargetType TargetType => SkillTargetType.Enemy;
        public override SkillType Type => SkillType.Melee;

        public override int UsageCount => 3;
    }
}