using System;

using Rpg.Client.Core.Skills;

namespace Rpg.Client.Core
{
    internal class CombatSkill
    {
        private readonly ICombatSkillContext _combatSkillContext;

        public CombatSkill(ISkill skill, CombatSkillEnv env, ICombatSkillContext combatSkillContext)
        {
            Skill = skill ?? throw new ArgumentNullException(nameof(skill));
            Env = env;
            _combatSkillContext = combatSkillContext ?? throw new ArgumentNullException(nameof(combatSkillContext));
        }

        public bool IsAvailable => IsCombatEnergyEnough();

        public ISkill Skill { get; }
        public CombatSkillEnv Env { get; }

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

    internal class CombatSkillEnv
    { 
        public CombatSkillCost Cost { get; set; }
        public CombatSkillEfficient Efficient { get; set; }
    }

    internal enum CombatSkillCost
    { 
        Free,
        Low,
        Normal,
        High
    }

    internal enum CombatSkillEfficient
    {
        Zero,
        Low,
        Normal,
        High
    }
}