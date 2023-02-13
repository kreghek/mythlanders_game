﻿namespace Core.Combats.Effects;

public sealed class ChangeStatEffect : IEffect
{
    public ChangeStatEffect(ITargetSelector selector, IEffectImposer imposer, UnitStatType statType, int value,
        Type lifetimeType)
    {
        TargetStatType = statType;
        Selector = selector;
        Imposer = imposer;
        Value = value;
        LifetimeType = lifetimeType;
    }

    public Type LifetimeType { get; }
    public UnitStatType TargetStatType { get; }
    public int Value { get; }

    public ITargetSelector Selector { get; }

    public IEffectImposer Imposer { get; }

    public void Influence(Combatant target, IEffectCombatContext context)
    {
        var lifetime = (ICombatantEffectLifetime)Activator.CreateInstance(LifetimeType)!;
        var combatantEffect = new ChangeStateCombatantEffect(lifetime, TargetStatType, Value);
        target.AddEffect(combatantEffect);
    }
}