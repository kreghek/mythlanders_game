namespace Core.Combats.CombatantEffects;

public class PassTurnCombatantEffect: ICombatantEffect
{
    public PassTurnCombatantEffect(ICombatantEffectSid sid, ICombatantEffectLifetime lifetime)
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
            // TODO Pass turn
        }
    }
}