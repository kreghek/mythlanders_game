﻿using System.Collections.Generic;

using Rpg.Client.Core.Effects;
using Rpg.Client.Models;

namespace Rpg.Client.Core.Skills
{
    internal class DefenseSkill : SkillBase
    {
        public DefenseSkill() : this(false)
        {
        }

        public DefenseSkill(bool costRequired) : base(
            new SkillVisualization
            {
                Type = SkillVisualizationStateType.Support,
                SoundEffectType = GameObjectSoundType.Defence
            }, costRequired)
        {
        }

        public override IEnumerable<EffectRule> Rules { get; } = new List<EffectRule>
        {
            new EffectRule
            {
                Direction = SkillDirection.Self,
                EffectCreator = new EffectCreator(u =>
                {
                    var effect = new DecreaseDamageEffect(multiplier: 0.5f) { Duration = 1 };

                    return effect;
                })
            }
        };

        public override string Sid => "Defense Stance";
        public override SkillTargetType TargetType => SkillTargetType.Friendly;
        public override SkillType Type => SkillType.None;
    }
}