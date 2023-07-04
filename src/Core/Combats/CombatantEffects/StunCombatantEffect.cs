namespace Core.Combats.CombatantEffects;

/// <summary>
/// Combatant with tis condition will pass next turn.
/// </summary>
public class StunCombatantEffect: ICombatantEffect
{
    public StunCombatantEffect(ICombatantEffectSid sid, ICombatantEffectLifetime lifetime)
    {
        Lifetime = lifetime;
        Sid = sid;
    }

    public ICombatantEffectLifetime Lifetime { get; }
    public ICombatantEffectSid Sid { get; }
    public void Dispel(Combatant combatant)
    {
        // Do nothing
    }

    public void Impose(Combatant combatant, ICombatantEffectImposeContext combatantEffectImposeContext)
    {
        // Do nothing
    }

    public void Update(CombatantEffectUpdateType updateType, ICombatantEffectLifetimeUpdateContext context)
    {
        if (updateType == CombatantEffectUpdateType.StartCombatantTurn)
        {
            context.CompleteTurn();
        }
    }
}