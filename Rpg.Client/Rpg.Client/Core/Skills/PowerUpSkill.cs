using System;
using System.Collections.Generic;

using Rpg.Client.Core.Effects;

namespace Rpg.Client.Core.Skills
{
    internal sealed class PowerUpSkill: SkillBase
    {
        public override IEnumerable<EffectRule> Rules { get; } = new List<EffectRule>
        {
            new EffectRule
            {
                Direction = SkillDirection.Target,
                EffectCreator = new EffectCreator(u =>
                {
                    var rawEffectValue = (double)u.Unit.Level / 5;

                    var effect = new PowerUpEffect
                    {
                        Value = (int)Math.Ceiling(rawEffectValue)
                    };

                    return effect;
                })
            }
        };

        public override string Sid => "Power Up";
        public override SkillTargetType TargetType => SkillTargetType.Friendly;
        public override SkillType Type => SkillType.Range;
    }
}
