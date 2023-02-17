using Core.Dices;

namespace Core.Combats.Effects;

public sealed class ChangeCurrentStatEffectInstance : EffectInstanceBase<ChangeCurrentStatEffect>
{
    public ChangeCurrentStatEffectInstance(ChangeCurrentStatEffect baseEffect): base(baseEffect)
    {
    }

    public override void Influence(Combatant target, IEffectCombatContext context)
    {
        var rolledValue = context.Dice.Roll(BaseEffect.StatValue.Min, BaseEffect.StatValue.Max);

        var statValue = target.Stats.Single(x => x.Type == BaseEffect.TargetStatType).Value;
        if (rolledValue > 0)
            statValue.Restore(rolledValue);
        else
            statValue.Consume(-rolledValue);

        context.NotifyCombatantDamaged(target, BaseEffect.TargetStatType, rolledValue);
    }
}