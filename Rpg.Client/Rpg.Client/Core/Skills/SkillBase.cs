using System.Collections.Generic;

namespace Rpg.Client.Core.Skills
{
    internal abstract class SkillBase
    {
        protected SkillBase() {
            Cost = null;
        }

        protected SkillBase(bool costRequired)
        {
            if (costRequired)
            {
                Cost = BASE_COST;
            }
        }

        public abstract IEnumerable<EffectRule> Rules { get; }
        public abstract string Sid { get; }

        public abstract SkillTargetType TargetType { get; }

        public abstract SkillType Type { get; }

        public int? Cost { get; }

        public int BASE_COST = 3;
    }
}