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

        public bool IsAvailable => CheckMana();

        public ISkill Skill { get; }
        public CombatSkillEnv Env { get; }

        private bool CheckMana()
        {
            if (Skill.ManaCost is null)
            {
                return true;
            }

            var currentMana = _combatSkillContext.GetMana();
            return currentMana >= Skill.ManaCost;
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