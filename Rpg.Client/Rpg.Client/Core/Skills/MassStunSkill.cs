using System;
using System.Collections.Generic;

using Rpg.Client.Core.Effects;

namespace Rpg.Client.Core.Skills
{
    internal class MassStunSkill : SkillBase
    {
        public MassStunSkill(): base(new SkillVisualization() { Type = SkillVisualizationStateType.Support })
        {
        }

        public MassStunSkill(bool costRequired) : base(new SkillVisualization() { Type = SkillVisualizationStateType.Support }, costRequired)
        {
        }

        public override IEnumerable<EffectRule> Rules { get; } = new List<EffectRule>
        {
            new EffectRule
            {
                Direction = SkillDirection.AllEnemy,
                EffectCreator = new EffectCreator(u =>
                {
                    var effect = new DopeHerbEffect();

                    effect.Value = (int)Math.Ceiling((double)u.Unit.Level / 5);

                    return effect;
                })
            }
        };

        public override string Sid => "Mass Stun";
        public override SkillTargetType TargetType => SkillTargetType.Enemy;
        public override SkillType Type => SkillType.Range;
    }
}