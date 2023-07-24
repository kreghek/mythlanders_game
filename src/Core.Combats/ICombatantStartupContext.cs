namespace Core.Combats;

public interface ICombatantStartupContext
{
    ICombatantStatusImposeContext ImposeStatusContext { get; }
    ICombatantStatusLifetimeImposeContext ImposeStatusLifetimeContext { get; }
}