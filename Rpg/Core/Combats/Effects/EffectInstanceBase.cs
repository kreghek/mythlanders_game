namespace Core.Combats.Effects;

public abstract class EffectInstanceBase<TEffect> : IEffectInstance where TEffect: IEffect
{
    public TEffect BaseEffect { get; }
    public ITargetSelector Selector => BaseEffect.Selector;

    protected EffectInstanceBase(TEffect baseEffect)
    {
        BaseEffect = baseEffect;
    }

    public virtual void AddModifier(IUnitStatModifier modifier)
    {
    }

    public abstract void Influence(Combatant target, IEffectCombatContext context);

    public virtual void RemoveModifier(IUnitStatModifier modifier)
    {
    }
}