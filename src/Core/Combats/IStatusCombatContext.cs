using Core.Dices;

namespace Core.Combats;

public interface IStatusCombatContext
{
    Combatant Actor { get; }
    IDice Dice { get; }

    ICombatantStatusImposeContext StatusImposedContext { get; }
    ICombatantStatusLifetimeImposeContext StatusLifetimeImposedContext { get; }
    CombatField Field { get; }

    int DamageCombatantStat(Combatant combatant, ICombatantStatType statType, int value);

    void NotifySwapFieldPosition(Combatant combatant, FieldCoords sourceCoords, CombatFieldSide sourceFieldSide,
        FieldCoords destinationCoords, CombatFieldSide destinationFieldSide);

    void PassTurn(Combatant target);
    void RestoreCombatantStat(Combatant combatant, ICombatantStatType statType, int value);
}