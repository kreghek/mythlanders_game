using Core.Combats;
using Core.Dices;

namespace Core.Combats.Effects;

public sealed class ChangeCurrentStatEffect : IEffect
{
    public ChangeCurrentStatEffect(ITargetSelector selector, UnitStatType statType,
        Range<int> statValue)
    {
        TargetStatType = statType;
        Selector = selector;
        StatValue = statValue;
    }

    public Range<int> StatValue { get; }
    public UnitStatType TargetStatType { get; }
    public ITargetSelector Selector { get; }

    public IEffectInstance CreateInstance()
    {
        return new ChangeCurrentStatEffectInstance(this);
    }
}

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

public sealed class ChangeCurrentStatEffectInstance : EffectInstanceBase<ChangeCurrentStatEffect>
{
    public ChangeCurrentStatEffectInstance(ChangeCurrentStatEffect baseEffect): base(baseEffect)
    {
    }

    public override void Influence(Combatant target, IEffectCombatContext context)
    {
        var rolledValue = context.Dice.Roll(BaseEffect.StatValue.Min, BaseEffect.StatValue.Max);

        var statValue = target.Stats.Single(x => x.Type == BaseEffect.TargetStatType).Value;
        if (rolledValue > 0)
            statValue.Restore(rolledValue);
        else
            statValue.Consume(rolledValue);

        context.NotifyCombatantDamaged(target, BaseEffect.TargetStatType, rolledValue);
    }
}
