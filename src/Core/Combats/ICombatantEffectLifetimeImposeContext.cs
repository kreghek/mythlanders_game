namespace Core.Combats;

public interface ICombatantEffectLifetimeImposeContext
{
    CombatCore Combat { get; }
}

public interface ICombatantEffectImposeContext
{
    CombatCore Combat { get; }
}