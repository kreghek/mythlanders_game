namespace Core.Combats.CombatantEffects;

public sealed class ChangeStatCombatantEffect : CombatantEffectBase
{
    private readonly IUnitStatModifier _statModifier;

    public ChangeStatCombatantEffect(ICombatantEffectLifetime lifetime, UnitStatType statType, int value): base(lifetime)
    {
        StatType = statType;
        Value = value;

        _statModifier = new StatModifier(value);
    }

    public UnitStatType StatType { get; }
    public int Value { get; }

    public override void Impose(Combatant combatant)
    {
        combatant.Stats.Single(x => x.Type == StatType).Value.AddModifier(_statModifier);
    }

    public override void Dispel(Combatant combatant)
    {
        combatant.Stats.Single(x => x.Type == StatType).Value.RemoveModifier(_statModifier);
    }
}

public interface ICombatantEffectFactory
{
    public ICombatantEffect Create();
}

public sealed class ModifyEffectsCombatantEffectFactory : ICombatantEffectFactory
{
    private readonly ICombatantEffectLifetimeFactory _lifetimeFactory;

    public ModifyEffectsCombatantEffectFactory(ICombatantEffectLifetimeFactory lifetimeFactory)
    {
        _lifetimeFactory = lifetimeFactory;
    }

    public ICombatantEffect Create()
    {
        throw new NotImplementedException();
        //return new ModifyEffectsCombatantEffect(_lifetimeFactory.Create(), );
    }
}