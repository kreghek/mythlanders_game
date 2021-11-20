using System;
using System.Collections.Generic;

using Rpg.Client.Core.Effects;
using Rpg.Client.Models;

namespace Rpg.Client.Core.Skills
{
    internal class PeriodicHealSkill : SkillBase
    {
        public PeriodicHealSkill() : this(false)
        {
        }

        public PeriodicHealSkill(bool costRequired) : base(new SkillVisualization
        { Type = SkillVisualizationStateType.Range, SoundEffectType = GameObjectSoundType.Heal }, costRequired)
        {
        }

        public override IEnumerable<EffectRule> Rules { get; } = new List<EffectRule>
        {
            new EffectRule
            {
                Direction = SkillDirection.Target,
                EffectCreator = new EffectCreator(u =>
                {
                    var effect = new PeriodicHealEffect
                    {
                        Duration = 3,
                        SourceSupport = (int)Math.Round(u.Unit.Power, MidpointRounding.AwayFromZero),
                        PowerMultiplier = 0.3f
                    };

                    return effect;
                })
            },
            new EffectRule
            {
                Direction = SkillDirection.Target,
                EffectCreator = new EffectCreator(u =>
                {
                    var effect = new HealEffect
                    {
                        Actor = u,
                        PowerMultiplier = 1,
                        Scatter = 1
                    };

                    return effect;
                })
            }
        };

        public override string Sid => "Periodic Heal";
        public override SkillTargetType TargetType => SkillTargetType.Friendly;
        public override SkillType Type => SkillType.Range;
    }
}