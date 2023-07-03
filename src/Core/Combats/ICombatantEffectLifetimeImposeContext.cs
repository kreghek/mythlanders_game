namespace Core.Combats;

public interface ICombatantEffectLifetimeImposeContext
{
    CombatCore Combat { get; }
    Combatant TargetCombatant { get; }
}

public interface ICombatantEffectImposeContext
{
    CombatCore Combat { get; }
}