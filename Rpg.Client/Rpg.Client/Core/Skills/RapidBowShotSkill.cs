﻿using System.Collections.Generic;
using System.Runtime.CompilerServices;

using Rpg.Client.Core.SkillEffects;
using Rpg.Client.GameScreens;

namespace Rpg.Client.Core.Skills
{
    internal class RapidBowShotSkill : SkillBase
    {
        public RapidBowShotSkill() : this(false)
        {
        }

        public RapidBowShotSkill(bool costRequired) : base(PredefinedVisualization, costRequired)
        {
        }

        public override IEnumerable<EffectRule> Rules { get; } = new List<EffectRule>
        {
            new EffectRule
            {
                Direction = SkillDirection.Target,
                EffectCreator = new EffectCreator(u =>
                {
                    var equipmentMultiplier = u.Unit.GetEquipmentAttackMultiplier(SkillSid.RapidEnergyShot);
                    var res = new AttackEffect
                    {
                        Actor = u,
                        DamageMultiplier = 1f * equipmentMultiplier,
                        Scatter = 0.5f
                    };

                    return res;
                })
            }
        };

        public override SkillSid Sid => SkillSid.RapidEnergyShot;
        public override SkillTargetType TargetType => SkillTargetType.Enemy;
        public override SkillType Type => SkillType.Range;

        private static SkillVisualization PredefinedVisualization => new()
        {
            Type = SkillVisualizationStateType.Range,
            SoundEffectType = GameObjectSoundType.BowShot
        };
    }
}