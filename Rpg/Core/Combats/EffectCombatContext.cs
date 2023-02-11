using Core.Dices;

namespace Core.Combats;

public sealed record EffectCombatContext(CombatField Field, IDice Dice, Action<Combatant, UnitStatType, int> NotifyCombatantDamagedDelegate) : IEffectCombatContext
{
    public void NotifyCombatantDamaged(Combatant combatant, UnitStatType statType, int value)
    {
        NotifyCombatantDamagedDelegate(combatant, statType, value);
    }
}
