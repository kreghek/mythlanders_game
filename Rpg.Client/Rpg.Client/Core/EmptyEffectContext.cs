namespace Rpg.Client.Core
{
    internal sealed class EmptyEffectContext : IEquipmentEffectContext
    {
        public EmptyEffectContext(CombatUnit combatUnit)
        {
            IsInTankingSlot = combatUnit.IsInTankLine;
        }
        
        public int EquipmentLevel { get; set; }
        public bool IsInTankingSlot { get; set; }
    }
}