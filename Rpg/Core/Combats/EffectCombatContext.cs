﻿using Core.Dices;

namespace Core.Combats;

public sealed class EffectCombatContext : IEffectCombatContext
{
    public EffectCombatContext(CombatField field,
        IDice dice,
        CombatantHasTakenDamagedCallback notifyCombatantDamagedDelegate,
        CombatantHasMovedCallback notifyCombatantMovedDelegate, 
        CombatCore combatCore)
    {
        Field = field;
        Dice = dice;
        NotifyCombatantDamagedDelegate = notifyCombatantDamagedDelegate;
        NotifyCombatantMovedDelegate = notifyCombatantMovedDelegate;

        EffectImposedContext = new CombatantEffectLifetimeImposeContext(combatCore);
    }

    public CombatantHasTakenDamagedCallback NotifyCombatantDamagedDelegate { get; }
    public CombatantHasMovedCallback NotifyCombatantMovedDelegate { get; }

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

    public ICombatantEffectLifetimeImposeContext EffectImposedContext { get; }

    public CombatField Field { get; }
    public IDice Dice { get; }
}