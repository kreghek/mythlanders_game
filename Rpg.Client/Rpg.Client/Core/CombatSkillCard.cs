using System;

using Rpg.Client.Core.Skills;

namespace Rpg.Client.Core
{
    internal class CombatSkillCard
    {
        public CombatSkillCard(SkillBase skill)
        {
            Skill = skill ?? throw new ArgumentNullException(nameof(skill));
        }

        public SkillBase Skill { get; }
    }
}