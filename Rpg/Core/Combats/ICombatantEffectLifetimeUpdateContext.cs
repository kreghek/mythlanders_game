namespace Core.Combats;

public interface ICombatantEffectLifetimeUpdateContext
{
    Combatant Combatant { get; }
}

public interface ICombatantEffectLifetimeImposeContext
{
    CombatCore Combat { get; }
}

public interface ICombatantEffectLifetimeDispelContext
{
    CombatCore Combat { get; }
}