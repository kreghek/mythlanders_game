using System.Collections.Generic;

using Rpg.Client.Core.Effects;

namespace Rpg.Client.Core.Skills
{
    internal class StrikeSkill : SkillBase
    {
        public StrikeSkill() : base(new SkillVisualization
        { Type = SkillVisualizationStateType.Range })
        {
        }

        public StrikeSkill(bool costRequired) : base(new SkillVisualization
        { Type = SkillVisualizationStateType.Range }, costRequired)
        {
        }

        public override IEnumerable<EffectRule> Rules { get; } = new List<EffectRule>
        {
            new EffectRule
            {
                Direction = SkillDirection.Target,
                EffectCreator = new EffectCreator(u =>
                {
                    var res = new AttackEffect
                    {
                        Actor = u,
                        PowerMultiplier = 1.5f,
                        ValueRange = 1
                    };

                    return res;
                })
            }
        };

        public override string Sid => "Strike";
        public override SkillTargetType TargetType => SkillTargetType.Enemy;
        public override SkillType Type => SkillType.Range;
    }
}