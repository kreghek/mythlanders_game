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

        public int RedEnergyCost => GetEnergyCost(Env.RedCost, Skill.BaseRedEnergyCost);
        
        public int GreenEnergyCost => GetEnergyCost(Env.GreenCost, Skill.BaseGreenEnergyCost);

        public int RedEnergyRegen => GetEnergyCost(Env.RedRegen, Skill.BaseRedEnergyCost / 2);

        public int GreenEnergyRegen => GetEnergyCost(Env.GreenRegen, Skill.BaseGreenEnergyCost / 2);

        private static int GetEnergyCost(CombatSkillCost combatSkillCost, int? baseEnergyCost)
        {
            if (baseEnergyCost is null)
            {
                return 0;
            }

            var coef = GetEnergyCoef(combatSkillCost);

            return (int)Math.Round(baseEnergyCost.Value * coef, MidpointRounding.AwayFromZero);
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
            var currentRedCombatEnergy = _combatSkillContext.GetRedCombatEnergy();
            var currentGreenCombatEnergy = _combatSkillContext.GetRedCombatEnergy();
            var redIsEnough = currentRedCombatEnergy >= RedEnergyCost;
            var greenIsEnough = currentGreenCombatEnergy >= GreenEnergyCost;
            return redIsEnough && greenIsEnough;
        }
    }

    internal class CombatSkillEnv
    { 
        public CombatSkillCost RedCost { get; set; }
        public CombatSkillCost GreenCost { get; set; }
        public CombatSkillCost RedRegen { get; set; }
        public CombatSkillCost GreenRegen { get; set; }
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