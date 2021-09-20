using System;
using System.Collections.Generic;

using Rpg.Client.Core.Effects;

namespace Rpg.Client.Core.Skills
{
    internal sealed class PowerUpSkill : SkillBase
    {
        public override IEnumerable<EffectRule> Rules { get; } = new List<EffectRule>
        {
            new EffectRule
            {
                Direction = SkillDirection.Target,
                EffectCreator = new EffectCreator(u =>
                {
                    // Multypli by 2 to a target unit be able to use effect.
                    // Otherwise, there is risk to deal with the problem. The target unit will attack after one-turn effect will expire.
                    var rawEffectDuration = (double)u.Unit.Level / 5 * 2;

                    var effect = new PowerUpEffect
                    {
                        Value = (int)Math.Ceiling(rawEffectDuration)
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