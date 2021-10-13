﻿using System.Collections.Generic;

namespace Rpg.Client.Core.Skills
{
    internal abstract class SkillBase : ISkill
    {
        public const int BASE_MANA_COST = 3;

        protected SkillBase(SkillVisualization visualization)
        {
            UsageCount = 1;
            Visualization = visualization;
        }

        protected SkillBase(SkillVisualization visualization, bool costRequired) : this(visualization)
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

        public SkillVisualization Visualization { get; }
    }
}