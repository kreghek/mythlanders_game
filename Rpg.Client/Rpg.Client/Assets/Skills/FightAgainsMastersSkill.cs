﻿using System.Collections.Generic;

using Rpg.Client.Core.SkillEffects;
using Rpg.Client.Core.Skills;
using Rpg.Client.GameScreens;

namespace Rpg.Client.Assets.Skills
{
    internal class FightAgainsMastersSkill : SkillBase
    {
        private const SkillSid SID = SkillSid.FightAgainstMaster;

        public FightAgainsMastersSkill() : this(false)
        {
        }

        public FightAgainsMastersSkill(bool costRequired) : base(PredefinedVisualization, costRequired)
        {
        }

        public override IEnumerable<EffectRule> Rules { get; } = new[]
        {
            new EffectRule
            {
                Direction = SkillDirection.Target,
                EffectCreator = new EffectCreator((u, env) =>
                {
                    var effect = new DecreaseDamageEffect(multiplier: 0.5f) { Duration = 1 };

                    return effect;
                })
            }
        };

        public override SkillSid Sid => SID;
        public override SkillTargetType TargetType => SkillTargetType.Friendly;
        public override SkillType Type => SkillType.Range;

        public override int UsageCount => 3;

        private static SkillVisualization PredefinedVisualization => new()
        {
            Type = SkillVisualizationStateType.Range,
            SoundEffectType = GameObjectSoundType.Heal
        };
    }
}