using Core.Combats.CombatantEffects;

namespace Core.Combats.Effects;

public sealed class ChangeStatEffect : IEffect
{
    public ChangeStatEffect(ITargetSelector selector, UnitStatType statType, int value,
        Type lifetimeType)
    {
        TargetStatType = statType;
        Selector = selector;
        Value = value;
        LifetimeType = lifetimeType;
    }

    public Type LifetimeType { get; }
    public UnitStatType TargetStatType { get; }
    public int Value { get; }

    public ITargetSelector Selector { get; }

    public IEffectInstance CreateInstance()
    {
        return new ChangeStatEffectInstance(this);
    }
}

public sealed class ChangeStatEffectInstance : EffectInstanceBase<ChangeStatEffect>
{
    public ChangeStatEffectInstance(ChangeStatEffect baseEffect): base(baseEffect)
    {
    }

    public override void Influence(Combatant target, IEffectCombatContext context)
    {
        var lifetime = (ICombatantEffectLifetime)Activator.CreateInstance(BaseEffect.LifetimeType)!;
        var combatantEffect = new ChangeStateCombatantEffect(lifetime, BaseEffect.TargetStatType, BaseEffect.Value);
        target.AddEffect(combatantEffect);
    }
}
