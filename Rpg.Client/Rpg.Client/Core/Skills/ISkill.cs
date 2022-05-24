using System.Collections.Generic;

namespace Rpg.Client.Core.Skills
{
    internal interface ISkill: IEffectSource
    {
        int? BaseEnergyCost { get; }
        IReadOnlyList<EffectRule> Rules { get; }
        SkillSid Sid { get; }
        SkillTargetType TargetType { get; }
        SkillType Type { get; }
    }
}