using Core.Dices;

namespace Core.Combats;

public sealed record EffectCombatContext(CombatField Field, IDice Dice,
    Action<Combatant, UnitStatType, int> NotifyCombatantDamagedDelegate,
    Action<Combatant, FieldCoords> NotifyCombatantMoved) : IEffectCombatContext
{
    public void NotifyCombatantDamaged(Combatant combatant, UnitStatType statType, int value)
    {
        NotifyCombatantDamagedDelegate(combatant, statType, value);
    }

    void IEffectCombatContext.NotifyCombatantMoved(Combatant combatant, FieldCoords coords)
    {
        NotifyCombatantMoved(combatant, coords);
    }
}