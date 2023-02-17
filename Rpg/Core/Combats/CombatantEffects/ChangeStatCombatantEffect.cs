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