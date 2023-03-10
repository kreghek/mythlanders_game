using Core.Dices;

namespace Core.Combats;

public sealed record EffectCombatContext(
    CombatField Field,
    IDice Dice,
    CombatantHasTakenDamagedCallback NotifyCombatantDamagedDelegate,
    CombatantHasMovedCallback NotifyCombatantMovedDelegate) : IEffectCombatContext
{
    public void NotifyCombatantDamaged(Combatant combatant, UnitStatType statType, int value)
    {
        NotifyCombatantDamagedDelegate(combatant, statType, value);
    }

    public void NotifySwapFieldPosition(Combatant combatant, FieldCoords sourceCoords, CombatFieldSide sourceFieldSide, FieldCoords destinationCoords, CombatFieldSide destinationFieldSide)
    {
        NotifyCombatantMovedDelegate(sourceCoords, sourceFieldSide, destinationCoords, destinationFieldSide);
    }
}