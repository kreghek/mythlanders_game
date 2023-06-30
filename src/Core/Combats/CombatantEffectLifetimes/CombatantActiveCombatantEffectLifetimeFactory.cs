namespace Core.Combats.CombatantEffectLifetimes;

public sealed class CombatantActiveCombatantEffectLifetimeFactory : ICombatantEffectLifetimeFactory
{
    private readonly Combatant _combatant;

    public CombatantActiveCombatantEffectLifetimeFactory(Combatant combatant)
    {
        _combatant = combatant;
    }

    public ICombatantEffectLifetime Create()
    {
        return new CombatantActiveCombatantEffectLifetime(_combatant);
    }
}