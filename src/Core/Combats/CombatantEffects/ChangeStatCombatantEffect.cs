namespace Core.Combats.CombatantEffects;

public sealed class ChangeStatCombatantEffect : CombatantEffectBase
{
    private readonly IUnitStatModifier _statModifier;

    public ChangeStatCombatantEffect(ICombatantEffectSid sid, ICombatantEffectLifetime lifetime, UnitStatType statType, int value) :
        base(sid, lifetime)
    {
        StatType = statType;
        Value = value;

        _statModifier = new StatModifier(value);
    }

    public UnitStatType StatType { get; }
    public int Value { get; }

    public override void Dispel(Combatant combatant)
    {
        combatant.Stats.Single(x => x.Type == StatType).Value.RemoveModifier(_statModifier);
    }

    public override void Impose(Combatant combatant, ICombatantEffectImposeContext combatantEffectImposeContext)
    {
        combatant.Stats.Single(x => x.Type == StatType).Value.AddModifier(_statModifier);
    }
}