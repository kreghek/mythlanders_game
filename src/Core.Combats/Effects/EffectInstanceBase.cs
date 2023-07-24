namespace Core.Combats.Effects;

public abstract class EffectInstanceBase<TEffect> : IEffectInstance where TEffect : IEffect
{
    protected EffectInstanceBase(TEffect baseEffect)
    {
        BaseEffect = baseEffect;
    }

    public TEffect BaseEffect { get; }
    public ITargetSelector Selector => BaseEffect.Selector;

    public virtual void AddModifier(IUnitStatModifier modifier)
    {
    }

    public abstract void Influence(Combatant target, IStatusCombatContext context);

    public virtual void RemoveModifier(IUnitStatModifier modifier)
    {
    }
}