namespace Core.Combats;

public interface ICombatantEffect
{
    ICombatantEffectLifetime Lifetime { get; }
    void Dispel(Combatant combatant);
    void Impose(Combatant combatant);
    void Update(CombatantEffectUpdateType updateType, ICombatantEffectLifetimeUpdateContext context);
}