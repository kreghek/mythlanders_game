using System.Collections.Generic;

using Rpg.Client.Core.Effects;

namespace Rpg.Client.Core.Skills
{
    internal sealed class PowerUpSkill : SkillBase
    {
        public PowerUpSkill() : base(new SkillVisualization
            { Type = SkillVisualizationStateType.Support })
        {
        }

        public PowerUpSkill(bool costRequired) : base(new SkillVisualization
            { Type = SkillVisualizationStateType.Support }, costRequired)
        {
        }

        public override IEnumerable<EffectRule> Rules { get; } = new List<EffectRule>
        {
            new EffectRule
            {
                Direction = SkillDirection.Target,
                EffectCreator = new EffectCreator(u =>
                {
                    var effect = new IncreaseAttackEffect(1.1f);
                    effect.Value = 3;

                    return effect;
                })
            }
        };

        public override string Sid => "Power Up";
        public override SkillTargetType TargetType => SkillTargetType.Friendly;
        public override SkillType Type => SkillType.Range;
    }
}