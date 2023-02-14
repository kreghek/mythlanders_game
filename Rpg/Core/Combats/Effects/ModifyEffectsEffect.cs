namespace Core.Combats.Effects;

public sealed class ModifyEffectsEffect: IEffect
{
    public ITargetSelector Selector { get; }
    public void Influence(Combatant target, IEffectCombatContext context)
    {
        throw new NotImplementedException();
    }
}

public sealed class ModifyEffectsEffectInstance : IEffectInstance
{
    private int _value;
    public ITargetSelector Selector { get; }
    public void Influence(Combatant target, IEffectCombatContext context)
    {
        target.AddEffect(new Com);
    }

    public void AddModifier(IUnitStatModifier modifier)
    {
        throw new NotImplementedException();
    }

    public void RemoveModifier(IUnitStatModifier modifier)
    {
        throw new NotImplementedException();
    }
}