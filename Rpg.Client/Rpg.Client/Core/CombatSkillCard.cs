using System;

namespace Rpg.Client.Core
{
    internal class CombatSkillCard
    {
        public CombatSkillCard(CombatSkill skill)
        {
            Skill = skill ?? throw new ArgumentNullException(nameof(skill));
        }

        public CombatSkill Skill { get; }
    }
}
