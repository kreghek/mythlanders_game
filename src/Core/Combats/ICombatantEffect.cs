namespace Core.Combats;

public interface ICombatantEffect
{
    ICombatantEffectLifetime Lifetime { get; }
    ICombatantEffectSid Sid { get; }
    void Dispel(Combatant combatant);
    void Impose(Combatant combatant, ICombatantEffectImposeContext combatantEffectImposeContext);
    void Update(CombatantEffectUpdateType updateType, ICombatantEffectLifetimeUpdateContext context);
}