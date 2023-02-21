using Core.Combats.CombatantEffects;

namespace Core.Combats.Effects;

public sealed class ChangeStatEffectInstance : EffectInstanceBase<ChangeStatEffect>
{
    public ChangeStatEffectInstance(ChangeStatEffect baseEffect) : base(baseEffect)
    {
    }

    public override void Influence(Combatant target, IEffectCombatContext context)
    {
        var lifetime = (ICombatantEffectLifetime)Activator.CreateInstance(BaseEffect.LifetimeType)!;
        var combatantEffect = new ChangeStatCombatantEffect(lifetime, BaseEffect.TargetStatType, BaseEffect.Value);
        target.AddEffect(combatantEffect);
    }
}