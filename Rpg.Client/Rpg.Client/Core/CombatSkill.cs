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

        public int EnergyCost => GetEnergyCost();

        private int GetEnergyCost()
        {
            if (Skill.CombatEnergyCost is null)
            {
                return 0;
            }

            var coef = GetEnergyCoef(Env.Cost);

            return (int)Math.Round(Skill.CombatEnergyCost.Value * coef, MidpointRounding.AwayFromZero);
        }

        private static float GetEnergyCoef(CombatSkillCost costEnv)
        {
            switch (costEnv)
            {
                case CombatSkillCost.Free: return 0;
                case CombatSkillCost.Low: return 0.5f;
                case CombatSkillCost.Normal: return 1;
                case CombatSkillCost.High: return 2;
                default: throw new Exception();
            }
        }

        public ISkill Skill { get; }
        public CombatSkillEnv Env { get; }

        private bool IsCombatEnergyEnough()
        {
            var currentCombatEnergy = _combatSkillContext.GetCombatEnergy();
            return currentCombatEnergy >= EnergyCost;
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