﻿using System.Collections.Generic;

using Rpg.Client.Core.SkillEffects;
using Rpg.Client.Core.Skills;
using Rpg.Client.GameScreens;

namespace Rpg.Client.Assets.Skills
{
    internal class VolkolakEnergySkill : SkillBase
    {
        public VolkolakEnergySkill() : this(false)
        {
        }

        public VolkolakEnergySkill(bool costRequired) : base(PredefinedVisualization,
            costRequired)
        {
        }

        public override IEnumerable<EffectRule> Rules { get; } = new List<EffectRule>
        {
            new EffectRule
            {
                Direction = SkillDirection.AllEnemies,
                EffectCreator = new EffectCreator(u =>
                {
                    var res = new DamageEffect
                    {
                        DamageMultiplier = 1.0f,
                        Actor = u
                    };

                    return res;
                })
            }
        };

        public override SkillSid Sid => SkillSid.VolkolakWarriorEnergyBurst;
        public override SkillTargetType TargetType => SkillTargetType.Enemy;
        public override SkillType Type => SkillType.Range;

        private static SkillVisualization PredefinedVisualization => new()
        {
            Type = SkillVisualizationStateType.Range,
            SoundEffectType = GameObjectSoundType.WispEnergy
        };
    }
}