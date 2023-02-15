using Core.Combats.CombatantEffectLifetimes;
using Core.Combats.CombatantEffects;

namespace Core.Combats.Effects;

public sealed class ModifyEffectsEffect: IEffect
{
    public ModifyEffectsEffect(ITargetSelector selector)
    {
        Selector = selector;
    }

    public ITargetSelector Selector { get; }

    public IEffectInstance CreateInstance()
    {
        return new ModifyEffectsEffectInstance(this);
    }
}

public sealed class ModifyEffectsEffectInstance : EffectInstanceBase<ModifyEffectsEffect>
{
    public ModifyEffectsEffectInstance(ModifyEffectsEffect baseEffect): base(baseEffect)
    {
    }

    public override void Influence(Combatant target, IEffectCombatContext context)
    {
        target.AddEffect(new ModifyEffectsCombatantEffect(new ToNextCombatantTurnEffectLifetime()));
    }
}