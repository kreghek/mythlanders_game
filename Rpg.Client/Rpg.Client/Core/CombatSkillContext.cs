namespace Rpg.Client.Core
{
    internal sealed class CombatSkillContext : ICombatSkillContext
    {
        private readonly CombatUnit _combatUnit;

        public CombatSkillContext(CombatUnit combatUnit)
        {
            _combatUnit = combatUnit;
        }

        public int GetRedCombatEnergy()
        {
            return _combatUnit.RedEnergyPool;
        }

        public int GetGreenCombatEnergy()
        {
            return _combatUnit.GreenEnergyPool;
        }
    }
}