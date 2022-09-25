namespace Rpg.Client.Core
{
    internal sealed class EquipmentEffectContext : IEquipmentEffectContext
    {
        public EquipmentEffectContext(CombatUnit combatUnit, Equipment equipment)
        {
            EquipmentLevel = equipment.Level;
            IsInTankingSlot = combatUnit.IsInTankLine;
        }

        public int EquipmentLevel { get; }
        public bool IsInTankingSlot { get; }
    }
}