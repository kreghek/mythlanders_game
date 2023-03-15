namespace Core.Combats;

public interface ICombatantEffectLifetime
{
    bool IsDead { get; }
    void Update(CombatantEffectUpdateType updateType, ICombatantEffectLifetimeUpdateContext context);
}