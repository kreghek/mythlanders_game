namespace Core.Combats.CombatantEffects;

public sealed class ModifyEffectsCombatantEffect : ICombatantEffect
{
    private readonly IUnitStatModifier _statModifier;

    public ModifyEffectsCombatantEffect(ICombatantEffectLifetime lifetime, int value)
    {
        Lifetime = lifetime;
        Value = value;
        _statModifier = new StatModifier(Value);
    }

    public int Value { get; }

    public ICombatantEffectLifetime Lifetime { get; }

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

    public void Impose(Combatant combatant)
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

    public void Update(CombatantEffectUpdateType updateType, ICombatantEffectLifetimeUpdateContext context)
    {
        Lifetime.Update(updateType, context);
    }
}