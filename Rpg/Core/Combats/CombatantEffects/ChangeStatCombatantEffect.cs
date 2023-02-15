namespace Core.Combats.CombatantEffects;

public sealed class ChangeStatCombatantEffect : ICombatantEffect
{
    private readonly IUnitStatModifier _statModifier;

    public ChangeStatCombatantEffect(ICombatantEffectLifetime lifetime, UnitStatType statType, int value)
    {
        StatType = statType;
        Value = value;
        Lifetime = lifetime;

        _statModifier = new StatModifier(value);
    }

    public UnitStatType StatType { get; }
    public int Value { get; }

    public void Impose(Combatant combatant)
    {
        combatant.Stats.Single(x => x.Type == StatType).Value.AddModifier(_statModifier);
    }

    public void Dispel(Combatant combatant)
    {
        combatant.Stats.Single(x => x.Type == StatType).Value.RemoveModifier(_statModifier);
    }

    public void Update(CombatantEffectUpdateType updateType)
    {
        Lifetime.Update(updateType);
    }

    public ICombatantEffectLifetime Lifetime { get; }
}

public sealed class ModifyEffectsCombatantEffect: ICombatantEffect
{
    private readonly IUnitStatModifier _statModifier;

    public ModifyEffectsCombatantEffect(ICombatantEffectLifetime lifetime, int value)
    {
        Lifetime = lifetime;
        Value = value;
        _statModifier = new StatModifier(Value);
    }

    public ICombatantEffectLifetime Lifetime { get; }

    public int Value { get; }

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

    public void Update(CombatantEffectUpdateType updateType)
    {
        Lifetime.Update(updateType);
    }
}