namespace Core.Combats.CombatantEffects;

public enum CombatantMoveStats
{
    None,
    Cost
}

public sealed class ModifyCombatantMoveStatsCombatantEffectFactory: ICombatantEffectFactory
{
    private readonly ICombatantEffectLifetimeFactory _lifetimeFactory;
    private readonly CombatantMoveStats _stats;
    private readonly int _value;

    public ModifyCombatantMoveStatsCombatantEffectFactory(ICombatantEffectLifetimeFactory lifetimeFactory, CombatantMoveStats stats, int value)
    {
        _lifetimeFactory = lifetimeFactory;
        _stats = stats;
        _value = value;
    }

    public ICombatantEffect Create()
    {
        return new ModifyCombatantMoveStatsCombatantEffect(_lifetimeFactory.Create(), _stats, _value);
    }
}

public sealed class ModifyCombatantMoveStatsCombatantEffect:CombatantEffectBase
{
    private readonly CombatantMoveStats _stats;
    private readonly int _value;
    private readonly StatModifier _modifier;

    public ModifyCombatantMoveStatsCombatantEffect(ICombatantEffectLifetime lifetime, CombatantMoveStats stats, int value) : base(lifetime)
    {
        _stats = stats;
        _value = value;
        
        _modifier = new StatModifier(_value);
    }

    public override void Impose(Combatant combatant)
    {
        base.Impose(combatant);

        foreach (var combatMovementInstance in combatant.Hand)
        {
            if (combatMovementInstance is not null)
            {
                switch (_stats)
                {
                    case CombatantMoveStats.Cost:
                        combatMovementInstance.Cost.Amount.AddModifier(_modifier);
                        break;
                }
                
            }
        }
    }

    public override void Dispel(Combatant combatant)
    {
        base.Dispel(combatant);
        
        foreach (var combatMovementInstance in combatant.Hand)
        {
            if (combatMovementInstance is not null)
            {
                switch (_stats)
                {
                    case CombatantMoveStats.Cost:
                        combatMovementInstance.Cost.Amount.RemoveModifier(_modifier);
                        break;
                }
                
            }
        }
    }
}

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
            if (combatMovementInstance is not null)
                foreach (var effectInstance in combatMovementInstance.Effects)
                    effectInstance.RemoveModifier(_statModifier);
    }

    public void Impose(Combatant combatant)
    {
        foreach (var combatMovementInstance in combatant.Hand)
            if (combatMovementInstance is not null)
                foreach (var effectInstance in combatMovementInstance.Effects)
                    effectInstance.AddModifier(_statModifier);
    }

    public void Update(CombatantEffectUpdateType updateType, ICombatantEffectLifetimeUpdateContext context)
    {
        Lifetime.Update(updateType, context);
    }
}