﻿using System.Collections.Generic;

using Rpg.Client.Core.Effects;
using Rpg.Client.Models;

namespace Rpg.Client.Core.Skills
{
    internal class ArrowRainSkill : SkillBase
    {
        public ArrowRainSkill() : this(false)
        {
        }

        public ArrowRainSkill(bool costRequired) : base(
            new SkillVisualization
            {
                Type = SkillVisualizationStateType.MassRange,
                SoundEffectType = GameObjectSoundType.BowShot
            }, costRequired)
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