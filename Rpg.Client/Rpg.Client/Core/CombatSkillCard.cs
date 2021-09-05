using System;

namespace Rpg.Client.Core
{
    public class CombatSkillCard
    {
        public CombatSkillCard(CombatSkill skill)
        {
            Skill = skill ?? throw new ArgumentNullException(nameof(skill));
        }

        public CombatSkill Skill { get; }
    }
}