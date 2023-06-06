namespace Core.Combats.Effects;

public sealed class InterruptEffectInstance : EffectInstanceBase<InterruptEffect>
{
    public InterruptEffectInstance(InterruptEffect stunEffect) : base(stunEffect)
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