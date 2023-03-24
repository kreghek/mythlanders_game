using Core.Dices;

namespace Core.Combats;

public sealed class EffectCombatContext : IEffectCombatContext
{
    private readonly ICombatantEffectLifetimeImposeContext _effectLifetimeContext;

    public EffectCombatContext(CombatField field,
        IDice dice,
        CombatantHasTakenDamagedCallback notifyCombatantDamagedDelegate,
        CombatantHasMovedCallback notifyCombatantMovedDelegate, CombatCore combatCore)
    {
        Field = field;
        Dice = dice;
        NotifyCombatantDamagedDelegate = notifyCombatantDamagedDelegate;
        NotifyCombatantMovedDelegate = notifyCombatantMovedDelegate;

        _effectLifetimeContext = new CombatantEffectLifetimeImposeContext(combatCore);
    }

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

    public ICombatantEffectLifetimeImposeContext EffectImposedContext => _effectLifetimeContext;
    public CombatField Field { get; }
    public IDice Dice { get; }
    public CombatantHasTakenDamagedCallback NotifyCombatantDamagedDelegate { get; }
    public CombatantHasMovedCallback NotifyCombatantMovedDelegate { get; }
}