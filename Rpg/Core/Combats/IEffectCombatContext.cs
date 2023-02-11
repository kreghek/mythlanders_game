using Core.Dices;

namespace Core.Combats;

public interface IEffectCombatContext
{
    CombatField Field { get; }

    IDice Dice { get; }

    void NotifyCombatantDamaged(Combatant combatant, UnitStatType statType, int value);
}
