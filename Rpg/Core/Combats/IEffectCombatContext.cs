using Core.Dices;

namespace Core.Combats;

public interface IEffectCombatContext
{
    IDice Dice { get; }
    CombatField Field { get; }

    void NotifyCombatantDamaged(Combatant combatant, UnitStatType statType, int value);
    
    void NotifySwapFieldPosition(Combatant combatant, FieldCoords sourceCoords, CombatFieldSide sourceFieldSide, FieldCoords destinationCoords, CombatFieldSide destinationFieldSide);
}