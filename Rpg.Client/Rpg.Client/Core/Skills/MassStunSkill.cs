using System;
using System.Collections.Generic;

using Rpg.Client.Core.Effects;
using Rpg.Client.Models;

namespace Rpg.Client.Core.Skills
{
    internal class MassStunSkill : SkillBase
    {
        public MassStunSkill() : this(false)
        {
        }

        public MassStunSkill(bool costRequired) : base(new SkillVisualization
                { Type = SkillVisualizationStateType.Support, SoundEffectType = GameObjectSoundType.EgyptianDarkMagic },
            costRequired)
        {
        }

        public override IEnumerable<EffectRule> Rules { get; } = new List<EffectRule>
        {
            new EffectRule
            {
                Direction = SkillDirection.AllEnemy,
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

        public override string Sid => "Mass Stun";
        public override SkillTargetType TargetType => SkillTargetType.Enemy;
        public override SkillType Type => SkillType.Range;
    }
}