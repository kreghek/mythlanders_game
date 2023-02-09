namespace Core.Combats.Effects;

public sealed class DefenseEffect : IEffect
{
    private readonly IUnitStatModifier _statModifier = new StatModifier(1);

    public ITargetSelector Selector { get; }

    public IEffectImposer Imposer { get; }

    public Range<int> Defense { get; }

    public DefenseEffect(ITargetSelector selector, IEffectImposer imposer, Range<int> defense)
    {
        Selector = selector;
        Imposer = imposer;
        Defense = defense;
    }

    public void Despel(Combatant target)
    {
        target.Stats.Single(x => x.Type == UnitStatType.Defense).Value.RemoveModifier(_statModifier);
    }

    public void Influence(Combatant target, IEffectCombatContext context)
    {
        target.Stats.Single(x => x.Type == UnitStatType.Defense).Value.AddModifier(_statModifier);
    }
}

public sealed record StatModifier(int Value) : IUnitStatModifier;

public sealed class ToRoundEndEffectImposer : IEffectImposer
{
    public void Impose(IEffect effect, Combatant target, IEffectCombatContext context)
    {
        target.AddEffect(effect);
    }

    public void Update(EffectImposerUpdateType updateType)
    {
        if (updateType == EffectImposerUpdateType.EndRound)
        { 
            
        }
    }
}