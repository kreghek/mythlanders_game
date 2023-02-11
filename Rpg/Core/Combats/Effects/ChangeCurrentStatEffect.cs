using Core.Dices;

namespace Core.Combats.Effects;

public sealed class ChangeCurrentStatEffect : IEffect
{
    public UnitStatType TargetStatType { get; }

    public ChangeCurrentStatEffect(ITargetSelector selector, IEffectImposer imposer, UnitStatType statType, Range<int> statValue)
    {
        TargetStatType = statType;
        Selector = selector;
        Imposer = imposer;
        StatValue = statValue;
    }

    public Range<int> StatValue { get; }
    public ITargetSelector Selector { get; }
    public IEffectImposer Imposer { get; }
    public void Influence(Combatant target, IEffectCombatContext context)
    {
        var rolledValue = context.Dice.Roll(StatValue.Min, StatValue.Max);

        var statValue = target.Stats.Single(x => x.Type == TargetStatType).Value;
        if (rolledValue > 0)
        {
            statValue.Restore(rolledValue);
        }
        else
        {
            statValue.Consume(rolledValue);
        }

        context.NotifyCombatantDamaged(target, TargetStatType, rolledValue);
    }
}