using System.Collections.Generic;

namespace Rpg.Client.Core.Skills
{
    internal abstract class SkillBase
    {
        public abstract IEnumerable<EffectRule> Rules { get; }
        public abstract string Sid { get; }

        public abstract SkillTargetType TargetType { get; }

        public abstract SkillType Type { get; }

        public int? Cost => 3;
    }
}