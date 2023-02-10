using Core.Dices;

namespace Core.Combats.Effects;

public sealed class ChangeCurrentStatEffect: IEffect
{
    private readonly UnitStatType _statType;

    public ChangeCurrentStatEffect(ITargetSelector selector, IEffectImposer imposer, UnitStatType statType, Range<int> statValue)
    {
        _statType = statType;
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
        target.Stats.Single(x=>x.Type == _statType).Value.Consume(rolledValue);
        
        context.NotifyCombatantDamaged(target, _statType, rolledValue);
    }

    public void Dispel(Combatant target)
    {
        throw new NotImplementedException();
    }
}