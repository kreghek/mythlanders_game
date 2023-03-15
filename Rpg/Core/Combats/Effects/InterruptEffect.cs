namespace Core.Combats.Effects;

public class InterruptEffect : IEffect
{
    public InterruptEffect(ITargetSelector selector)
    {
        Selector = selector;
    }

    public ITargetSelector Selector { get; }

    public IReadOnlyCollection<IEffectCondition> ImposeConditions => Array.Empty<IEffectCondition>();

    public IEffectInstance CreateInstance()
    {
        return new InterruptEffectInstance(this);
    }
}

public sealed class InterruptEffectInstance : EffectInstanceBase<InterruptEffect>
{
    public InterruptEffectInstance(InterruptEffect stunffect) : base(stunffect)
    {

    }



    public override void AddModifier(IUnitStatModifier modifier)
    {

    }

    public override void Influence(Combatant target, IEffectCombatContext context)
    {
        context.PassTurn(target);
    }

    public override void RemoveModifier(IUnitStatModifier modifier)
    {

    }
}