using System.Collections.Generic;

namespace Rpg.Client.Core.Skills
{
    internal abstract class SkillBase
    {
        public int BASE_COST = 3;

        protected SkillBase()
        {
            Cost = null;
        }

        protected SkillBase(bool costRequired)
        {
            if (costRequired)
            {
                Cost = BASE_COST;
            }
        }

        public int? Cost { get; }

        public abstract IEnumerable<EffectRule> Rules { get; }
        public abstract string Sid { get; }

        public abstract SkillTargetType TargetType { get; }

        public abstract SkillType Type { get; }
    }
}