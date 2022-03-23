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

        public bool IsAvailable => IsCombatEnergyEnough();

        public ISkill Skill { get; }

        private bool IsCombatEnergyEnough()
        {
            if (Skill.CombatEnergyCost is null)
            {
                return true;
            }

            var currentCombatEnergy = _combatSkillContext.GetCombatEnergy();
            return currentCombatEnergy >= Skill.CombatEnergyCost;
        }
    }
}