namespace Core.Combats;

public interface ICombatantEffect
{
    ICombatantEffectSid Sid { get; }
    ICombatantEffectLifetime Lifetime { get; }
    void Dispel(Combatant combatant);
    void Impose(Combatant combatant, ICombatantEffectImposeContext combatantEffectImposeContext);
    void Update(CombatantEffectUpdateType updateType, ICombatantEffectLifetimeUpdateContext context);
}