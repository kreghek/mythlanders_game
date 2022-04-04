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

        public int EnergyCost => GetEnergyCost(Skill.BaseEnergyCost);

        private static int GetEnergyCost(int? baseEnergyCost)
        {
            if (baseEnergyCost is null)
            {
                return 0;
            }

            return baseEnergyCost.Value;
        }

        public ISkill Skill { get; }

        private bool IsCombatEnergyEnough()
        {
            var currentRedCombatEnergy = _combatSkillContext.GetRedCombatEnergy();
            var redIsEnough = currentRedCombatEnergy >= EnergyCost;
            return redIsEnough;
        }
    }
}