using System.Collections.Generic;

namespace Rpg.Client.Core.Skills
{
    internal abstract class SkillBase
    {
        public abstract string Sid { get; }

        public abstract SkillType Type { get; }

        public abstract SkillTargetType TargetType { get; }

        public abstract IEnumerable<EffectRule> Rules { get; }
    }
}