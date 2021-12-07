using System;

using Rpg.Client.Core.Skills;

namespace Rpg.Client.Core
{
    internal class CombatSkill
    {
        private readonly ICombatSkillContext _combatSkillContext;

        public CombatSkill(ISkill skill, ICombatSkillContext combatSkillContext)
        {
            Skill = skill ?? throw new ArgumentNullException(nameof(skill));
            _combatSkillContext = combatSkillContext ?? throw new ArgumentNullException(nameof(combatSkillContext));
        }

        public bool IsAvailable
        {
            get
            {
                return CheckMana();
            }
        }

        private bool CheckMana()
        {
            if (Skill.ManaCost is null)
            {
                return true;
            }

            var currentMana = _combatSkillContext.GetMana();
            return currentMana >= Skill.ManaCost;
        }

        public ISkill Skill { get; }
    }
}