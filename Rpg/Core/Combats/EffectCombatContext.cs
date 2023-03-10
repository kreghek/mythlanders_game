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

    public void NotifyCombatantMoved(Combatant combatant, FieldCoords coords)
    {
        NotifyCombatantMovedDelegate(combatant, coords);
    }
}