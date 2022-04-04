using System.Collections.Generic;

using Rpg.Client.Core.Skills;

namespace Rpg.Client.Core
{
    internal sealed class VoiceSkill : ISkill
    {
        public int? BaseEnergyCost { get; }
        public IEnumerable<EffectRule> Rules { get; }
        public SkillSid Sid { get; }
        public SkillTargetType TargetType { get; }
        public SkillType Type { get; }
        public int UsageCount { get; }
        public SkillVisualization Visualization { get; }
    }
}