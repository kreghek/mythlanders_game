using Core.Dices;

namespace Core.Combats;

public sealed class EffectCombatContext : IEffectCombatContext
{
    private readonly ICombatantEffectLifetimeImposeContext _effectLifetimeContext;

    public EffectCombatContext(CombatField Field,
        IDice Dice,
        CombatantHasTakenDamagedCallback NotifyCombatantDamagedDelegate,
        CombatantHasMovedCallback NotifyCombatantMovedDelegate, CombatCore combatCore)
    {
        this.Field = Field;
        this.Dice = Dice;
        this.NotifyCombatantDamagedDelegate = NotifyCombatantDamagedDelegate;
        this.NotifyCombatantMovedDelegate = NotifyCombatantMovedDelegate;
        this.combatCore = combatCore;

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
    public CombatField Field { get; init; }
    public IDice Dice { get; init; }
    public CombatantHasTakenDamagedCallback NotifyCombatantDamagedDelegate { get; init; }
    public CombatantHasMovedCallback NotifyCombatantMovedDelegate { get; init; }
    public CombatCore combatCore { get; init; }
}