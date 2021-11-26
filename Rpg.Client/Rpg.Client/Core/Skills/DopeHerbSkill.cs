using System;
using System.Collections.Generic;

using Rpg.Client.Core.SkillEffects;
using Rpg.Client.GameScreens;

namespace Rpg.Client.Core.Skills
{
    internal class DopeHerbSkill : SkillBase
    {
        public DopeHerbSkill() : this(false)
        {
        }

        public DopeHerbSkill(bool costRequired) : base(PredefinedVisualization, costRequired)
        {
        }

        public override IEnumerable<EffectRule> Rules { get; } = new List<EffectRule>
        {
            new EffectRule
            {
                Direction = SkillDirection.Target,
                EffectCreator = new EffectCreator(u =>
                {
                    var effect = new StunEffect
                    {
                        Duration = 1
                    };

                    return effect;
                })
            }
        };

        public override SkillSid Sid => SkillSid.DopeHerb;
        public override SkillTargetType TargetType => SkillTargetType.Enemy;
        public override SkillType Type => SkillType.Range;

        private static SkillVisualization PredefinedVisualization => new()
        {
            Type = SkillVisualizationStateType.Support,
            SoundEffectType = GameObjectSoundType.MagicDust
        };
    }
}