﻿using System.Collections.Generic;

using Rpg.Client.Core.SkillEffects;
using Rpg.Client.GameScreens;

namespace Rpg.Client.Core.Skills
{
    internal class HealingSalveSkill : SkillBase
    {
        public HealingSalveSkill() : this(false)
        {
        }

        public HealingSalveSkill(bool costRequired) : base(PredefinedVisualization, costRequired)
        {
        }

        public override IEnumerable<EffectRule> Rules { get; } = new[]
        {
            new EffectRule
            {
                Direction = SkillDirection.Target,
                EffectCreator = new EffectCreator(u =>
                {
                    var effect = new HealEffect
                    {
                        SourceSupport = u.Unit.Support,
                        PowerMultiplier = 0.3f
                    };

                    return effect;
                })
            }
        };

        public override SkillSid Sid => SkillSid.HealingSalve;
        public override SkillTargetType TargetType => SkillTargetType.Friendly;
        public override SkillType Type => SkillType.Range;

        private static SkillVisualization PredefinedVisualization => new()
        {
            Type = SkillVisualizationStateType.Range,
            SoundEffectType = GameObjectSoundType.Heal
        };
    }
}