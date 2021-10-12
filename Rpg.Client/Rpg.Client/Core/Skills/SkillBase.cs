using System.Collections.Generic;

namespace Rpg.Client.Core.Skills
{
    internal abstract class SkillBase
    {
        public const int BASE_MANA_COST = 3;

        protected SkillBase()
        {
            UsageCount = 1;
        }

        protected SkillBase(bool costRequired): this()
        {
            if (costRequired)
            {
                ManaCost = BASE_MANA_COST;
            }
        }

        public int? ManaCost { get; }

        public abstract IEnumerable<EffectRule> Rules { get; }

        public abstract string Sid { get; }

        public abstract SkillTargetType TargetType { get; }

        public abstract SkillType Type { get; }

        public virtual int UsageCount { get; }
    }
}