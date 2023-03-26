using Core.Combats.CombatantEffects;

namespace Core.Combats.Effects;

public sealed class MarkEffect : IEffect
{
    private readonly ICombatantEffectLifetimeFactory _combatantEffectLifetimeFactory;

    public MarkEffect(ITargetSelector selector, ICombatantEffectLifetimeFactory combatantEffectLifetimeFactory)
    {
        _combatantEffectLifetimeFactory = combatantEffectLifetimeFactory;
        Selector = selector;
    }
    public IReadOnlyCollection<IEffectCondition> ImposeConditions => Array.Empty<IEffectCondition>();
    public ITargetSelector Selector { get; }
    public IEffectInstance CreateInstance()
    {
        return new MarkEffectInstance(this, _combatantEffectLifetimeFactory.Create());
    }
}

public sealed class MarkEffectInstance : EffectInstanceBase<MarkEffect>
{
    private readonly ICombatantEffectLifetime _lifetime;

    public MarkEffectInstance(MarkEffect baseEffect, ICombatantEffectLifetime lifetime) : base(baseEffect)
    {
        _lifetime = lifetime;
    }

    public override void Influence(Combatant target, IEffectCombatContext context)
    {
        target.AddEffect(new MarkCombatantEffect(_lifetime), context.EffectImposedContext);
    }
}

public sealed class DamageEffect : IEffect
{
    public DamageEffect(ITargetSelector selector, DamageType damageType, Range<int> damage)
    {
        Selector = selector;
        DamageType = damageType;
        Damage = damage;
        Modifiers = ArraySegment<IDamageEffectModifier>.Empty;
    }

    public DamageEffect(ITargetSelector selector, DamageType damageType, Range<int> damage, IReadOnlyList<IDamageEffectModifier> modifiers)
    {
        Selector = selector;
        DamageType = damageType;
        Damage = damage;
        Modifiers = modifiers;
    }

    public Range<int> Damage { get; }
    public DamageType DamageType { get; }
    public IReadOnlyList<IDamageEffectModifier> Modifiers { get; }

    public ITargetSelector Selector { get; }

    public IReadOnlyCollection<IEffectCondition> ImposeConditions => Array.Empty<IEffectCondition>();

    public IEffectInstance CreateInstance()
    {
        return new DamageEffectInstance(this);
    }
}

public interface IDamageEffectModifier
{
    Range<int> Process(Range<int> damage);
}

public sealed class MarkDamageEffectModifier : IDamageEffectModifier
{
    private readonly int _bonus;

    public MarkDamageEffectModifier(int bonus)
    {
        _bonus = bonus;
    }

    public Range<int> Process(Range<int> damage)
    {
        return new Range<int>(damage.Min + _bonus, damage.Max + _bonus);
    }
}