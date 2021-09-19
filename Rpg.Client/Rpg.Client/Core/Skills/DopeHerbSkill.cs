using System;
using System.Collections.Generic;

using Rpg.Client.Core.Effects;

namespace Rpg.Client.Core.Skills
{
    internal class DopeHerbSkill : SkillBase
    {
        public override IEnumerable<EffectRule> Rules { get; } = new List<EffectRule>
        {
            new EffectRule
            {
                Direction = SkillDirection.Target,
                EffectCreator = new EffectCreator(u =>
                {
                    var effect = new DopeHerbEffect();

                    effect.Value = (int)Math.Ceiling((double)u.Unit.Level / 5);

                    return effect;
                })
            }
        };

        public override string Sid => "Dope Herb";
        public override SkillTargetType TargetType => SkillTargetType.Enemy;
        public override SkillType Type => SkillType.Range;
    }

    internal class MassStunSkill : SkillBase
    {
        public override IEnumerable<EffectRule> Rules { get; } = new List<EffectRule>
        {
            new EffectRule
            {
                Direction = SkillDirection.AllEnemy,
                EffectCreator = new EffectCreator(u =>
                {
                    var effect = new DopeHerbEffect();

                    effect.Value = (int)Math.Ceiling((double)u.Unit.Level / 5);

                    return effect;
                })
            }
        };

        public override string Sid => "Mass Stun";
        public override SkillTargetType TargetType => SkillTargetType.Enemy;
        public override SkillType Type => SkillType.Range;
    }
}