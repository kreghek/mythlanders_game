using Core.Dices;

namespace Core.Combats;

public interface IEffectCombatContext
{
    IDice Dice { get; }
    CombatField Field { get; }

    void NotifyCombatantDamaged(Combatant combatant, UnitStatType statType, int value);
}