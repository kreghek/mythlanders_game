namespace Core.Combats.Effects;

public sealed class PeriodicEffectInstance : EffectInstanceBase<PeriodicEffect>
{
    public PeriodicEffectInstance(PeriodicEffect baseEffect) : base(baseEffect)
    {
    }

    public override void Influence(Combatant target, IEffectCombatContext context)
    {
        throw new NotImplementedException();
    }
}
