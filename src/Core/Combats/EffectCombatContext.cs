using Core.Dices;

namespace Core.Combats;

public sealed class EffectCombatContext : IEffectCombatContext
{
    public EffectCombatContext(
        Combatant targetCombatant,
        CombatField field,
        IDice dice,
        CombatantHasTakenDamagedCallback notifyCombatantDamagedDelegate,
        CombatantHasMovedCallback notifyCombatantMovedDelegate,
        CombatCore combatCore)
    {
        Actor = targetCombatant;
        Field = field;
        Dice = dice;
        NotifyCombatantDamagedDelegate = notifyCombatantDamagedDelegate;
        NotifyCombatantMovedDelegate = notifyCombatantMovedDelegate;

        EffectImposedContext = new CombatantEffectImposeContext(combatCore);
        EffectLifetimeImposedContext = new CombatantEffectLifetimeImposeContext(targetCombatant, combatCore);
    }

    public CombatantHasTakenDamagedCallback NotifyCombatantDamagedDelegate { get; }
    public CombatantHasMovedCallback NotifyCombatantMovedDelegate { get; }

    public Combatant Actor { get; }

    public int DamageCombatantStat(Combatant combatant, ICombatantStatType statType, int value)
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
    }

    public void RestoreCombatantStat(Combatant combatant, ICombatantStatType statType, int value)
    {
        throw new NotImplementedException();
    }

    public ICombatantStatusLifetimeImposeContext EffectLifetimeImposedContext { get; }

    public ICombatantStatusImposeContext EffectImposedContext { get; }

    public CombatField Field { get; }
    public IDice Dice { get; }
}