namespace Core.Combats.CombatantStatus;

/// <summary>
/// Change combat movements effects. Damage, etc.
/// </summary>
public sealed class ModifyEffectsCombatantStatus : ICombatantStatus
{
    private readonly IUnitStatModifier _statModifier;

    public ModifyEffectsCombatantStatus(ICombatantStatusSid sid, ICombatantStatusLifetime lifetime, int value)
    {
        Sid = sid;
        Lifetime = lifetime;
        Value = value;
        _statModifier = new StatModifier(Value);
    }

    public int Value { get; }

    public ICombatantStatusSid Sid { get; }
    public ICombatantStatusLifetime Lifetime { get; }

    public void Dispel(Combatant combatant)
    {
        foreach (var combatMovementInstance in combatant.Hand)
        {
            if (combatMovementInstance is not null)
            {
                foreach (var effectInstance in combatMovementInstance.Effects)
                {
                    effectInstance.RemoveModifier(_statModifier);
                }
            }
        }
    }

    public void Impose(Combatant combatant, ICombatantStatusImposeContext combatantEffectImposeContext)
    {
        foreach (var combatMovementInstance in combatant.Hand)
        {
            if (combatMovementInstance is not null)
            {
                foreach (var effectInstance in combatMovementInstance.Effects)
                {
                    effectInstance.AddModifier(_statModifier);
                }
            }
        }
    }

    public void Update(CombatantStatusUpdateType updateType, ICombatantStatusLifetimeUpdateContext context)
    {
        Lifetime.Update(updateType, context);
    }
}