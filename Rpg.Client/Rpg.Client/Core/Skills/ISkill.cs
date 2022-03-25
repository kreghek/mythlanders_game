using System.Collections.Generic;

namespace Rpg.Client.Core.Skills
{
    internal interface ISkill
    {
        int? BaseEnergyCost { get; }
        IEnumerable<EffectRule> Rules { get; }
        SkillSid Sid { get; }
        SkillTargetType TargetType { get; }
        SkillType Type { get; }
        int UsageCount { get; }
        SkillVisualization Visualization { get; }
        int Weight { get; }
    }
}