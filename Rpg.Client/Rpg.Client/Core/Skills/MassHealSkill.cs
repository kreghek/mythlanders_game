using System.Collections.Generic;

using Rpg.Client.Core.Effects;

namespace Rpg.Client.Core.Skills
{
    internal class MassHealSkill : SkillBase
    {
        public MassHealSkill()
        {
        }

        public MassHealSkill(bool costRequired) : base(costRequired)
        {
        }

        public override IEnumerable<EffectRule> Rules { get; } = new List<EffectRule>
        {
            new EffectRule
            {
                Direction = SkillDirection.AllFriendly,
                EffectCreator = new EffectCreator(u =>
                {
                    var effect = new HealEffect
                    {
                        Actor = u,
                        PowerMultiplier = 0.7f,
                        ValueRange = 1
                    };

                    return effect;
                })
            }
        };

        public override string Sid => "Mass Heal";
        public override SkillTargetType TargetType => SkillTargetType.Friendly;
        public override SkillType Type => SkillType.Telekinetic;
    }
}