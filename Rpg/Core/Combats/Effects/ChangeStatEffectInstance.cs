using Core.Combats.CombatantEffects;

namespace Core.Combats.Effects;

public sealed class ChangeStatEffectInstance : EffectInstanceBase<ChangeStatEffect>
{
    private readonly ICombatantEffectLifetime _combatantEffectLifetime;

    public ICombatantEffectLifetime Lifetime => _combatantEffectLifetime;

    public ChangeStatEffectInstance(ChangeStatEffect baseEffect, ICombatantEffectLifetime combatantEffectLifetime) :
        base(baseEffect)
    {
        _combatantEffectLifetime = combatantEffectLifetime;
    }

    public override void Influence(Combatant target, IEffectCombatContext context)
    {
        var combatantEffect =
            new ChangeStatCombatantEffect(_combatantEffectLifetime, BaseEffect.TargetStatType, BaseEffect.Value);
        target.AddEffect(combatantEffect);
    }
}