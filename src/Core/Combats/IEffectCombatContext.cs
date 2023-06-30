﻿using Core.Dices;

namespace Core.Combats;

public interface IEffectCombatContext
{
    Combatant Actor { get; }
    IDice Dice { get; }

    ICombatantEffectImposeContext EffectImposedContext { get; }
    ICombatantEffectLifetimeImposeContext EffectLifetimeImposedContext { get; }
    CombatField Field { get; }

    int DamageCombatantStat(Combatant combatant, UnitStatType statType, int value);

    void NotifySwapFieldPosition(Combatant combatant, FieldCoords sourceCoords, CombatFieldSide sourceFieldSide,
        FieldCoords destinationCoords, CombatFieldSide destinationFieldSide);

    void PassTurn(Combatant target);
    void RestoreCombatantStat(Combatant combatant, UnitStatType statType, int value);
}