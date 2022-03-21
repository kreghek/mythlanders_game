﻿using System.Collections.Generic;

using Rpg.Client.Core.SkillEffects;
using Rpg.Client.Core.Skills;
using Rpg.Client.GameScreens;

namespace Rpg.Client.Assets.Skills
{
    internal class SuperNaturalAgilitySkill : SkillBase
    {
        public SuperNaturalAgilitySkill(bool costRequired) : base(PredefinedVisualization, costRequired)
        {
        }

        public override IEnumerable<EffectRule> Rules { get; } = new List<EffectRule>
        {
            new EffectRule
            {
                Direction = SkillDirection.Self,
                EffectCreator = new EffectCreator((u, env) =>
                {
                    var effect = new DecreaseDamageEffect(multiplier: 0.5f) { Duration = 5 };

                    return effect;
                })
            }
        };

        public override SkillSid Sid => SkillSid.SuperNaturalAgility;
        public override SkillTargetType TargetType => SkillTargetType.Self;
        public override SkillType Type => SkillType.None;

        private static SkillVisualization PredefinedVisualization => new()
        {
            Type = SkillVisualizationStateType.Support,
            SoundEffectType = GameObjectSoundType.Defence
        };
    }
}