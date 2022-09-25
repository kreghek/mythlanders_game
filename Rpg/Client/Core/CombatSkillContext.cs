namespace Rpg.Client.Core
{
    internal sealed class CombatSkillContext : ICombatSkillContext
    {
        private readonly ICombatUnit _combatUnit;

        public CombatSkillContext(ICombatUnit combatUnit)
        {
            _combatUnit = combatUnit;
        }

        public int GetRedCombatEnergy()
        {
            return _combatUnit.EnergyPool;
        }
    }
}