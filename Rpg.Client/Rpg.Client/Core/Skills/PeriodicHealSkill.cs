using System;
using System.Collections.Generic;

using Rpg.Client.Core.Effects;
using Rpg.Client.Models;

namespace Rpg.Client.Core.Skills
{
    internal class PeriodicHealSkill : SkillBase
    {
        private static SkillVisualization PredefinedVisualization => new()
        {
            Type = SkillVisualizationStateType.Range, 
            SoundEffectType = GameObjectSoundType.Heal
        };
        
        public PeriodicHealSkill() : this(false)
        {
        }

        public PeriodicHealSkill(bool costRequired) : base(PredefinedVisualization, costRequired)
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
                        SourceSupport = u.Unit.Support,
                        PowerMultiplier = 0.3f
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