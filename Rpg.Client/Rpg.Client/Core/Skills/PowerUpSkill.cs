using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework.Graphics;

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
                    var effect = new IncreaseAttackEffect(1.1f);
                    effect.Value = 3;

                    return effect;
                })
            }
        };

        public override string Sid => "Power Up";
        public override SkillTargetType TargetType => SkillTargetType.Friendly;
        public override SkillType Type => SkillType.Range;
    }
}