using System.Collections.Generic;

namespace Rpg.Client.Core.Skills
{
    internal abstract class SkillBase : ISkill
    {
        public const int BASE_MANA_COST = 3;

        protected const int BASE_WEIGHT = 5;

        protected SkillBase(SkillVisualization visualization)
        {
            UsageCount = 1;
            Visualization = visualization;
        }

        protected SkillBase(SkillVisualization visualization, bool costRequired) : this(visualization)
        {
            if (costRequired)
            {
                BaseEnergyCost = BASE_MANA_COST;
                BaseGreenEnergyCost = BASE_MANA_COST;
            }
        }

        public int? BaseEnergyCost { get; }
        public int? BaseGreenEnergyCost { get; }


        public abstract IEnumerable<EffectRule> Rules { get; }

        public abstract SkillSid Sid { get; }

        public abstract SkillTargetType TargetType { get; }

        public abstract SkillType Type { get; }

        public virtual int UsageCount { get; }

        public SkillVisualization Visualization { get; }
        public virtual int Weight => BASE_WEIGHT;
    }
}