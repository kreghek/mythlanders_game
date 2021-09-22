﻿using System.Collections.Generic;

using Rpg.Client.Core.Effects;

namespace Rpg.Client.Core.Skills
{
    internal class DefenseSkill : SkillBase
    {
        public DefenseSkill()
        {
        }

        public DefenseSkill(bool costRequired) : base(costRequired)
        {
        }

        public override IEnumerable<EffectRule> Rules { get; } = new List<EffectRule>
        {
            new EffectRule
            {
                Direction = SkillDirection.Target,
                EffectCreator = new EffectCreator(u =>
                {
                    var effect = new DecreaseDamageEffect(0.5f) { Value = 1 };

                    return effect;
                })
            }
        };

        public override string Sid => "Defense Stance";
        public override SkillTargetType TargetType => SkillTargetType.Friendly;
        public override SkillType Type => SkillType.None;
    }
}