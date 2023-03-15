using Core.Dices;

namespace Core.Combats;

public sealed record EffectCombatContext(
    CombatField Field,
    IDice Dice,
    CombatantHasTakenDamagedCallback NotifyCombatantDamagedDelegate,
    CombatantHasMovedCallback NotifyCombatantMovedDelegate) : IEffectCombatContext
{
    public Combatant Actor => throw new NotImplementedException();

    public int DamageCombatantStat(Combatant combatant, UnitStatType statType, int value)
    {
        return NotifyCombatantDamagedDelegate(combatant, statType, value);
    }

    public void NotifySwapFieldPosition(Combatant combatant, FieldCoords sourceCoords, CombatFieldSide sourceFieldSide,
        FieldCoords destinationCoords, CombatFieldSide destinationFieldSide)
    {
        NotifyCombatantMovedDelegate(sourceCoords, sourceFieldSide, destinationCoords, destinationFieldSide);
    }

    public void PassTurn(Combatant target)
    {
        throw new NotImplementedException();
    }

    public void RestoreCombatantStat(Combatant combatant, UnitStatType statType, int value)
    {
        throw new NotImplementedException();
    }
}