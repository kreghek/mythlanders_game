using Core.Dices;

namespace Core.Combats.Effects;

public sealed class DamageEffect: IEffect
{
    public DamageEffect(ITargetSelector selector, IEffectImposer imposer, Range<int> damage)
    {
        Selector = selector;
        Imposer = imposer;
        Damage = damage;
    }

    public ITargetSelector Selector { get; }

    public IEffectImposer Imposer { get; }

    public Range<int> Damage { get; }

    public void Influence(Combatant target, IEffectCombatContext context)
    {
        var targetDefenseMovement = GetAutoDefenseMovement(target);
        
        if 
        
        var rolledDamage = context.Dice.Roll(Damage.Min, Damage.Max);

        var damageRemains = TakeStat(target, UnitStatType.ShieldPoints, rolledDamage);

        if (damageRemains > 0)
        {
            TakeStat(target, UnitStatType.HitPoints, damageRemains);
        }
    }

    private CombatMovement? GetAutoDefenseMovement(Combatant target)
    {
        return target.Hand.First(x => x.Tags.HasFlag(CombatMovementTags.AutoDefense));
    }

    private static int TakeStat(Combatant combatant, UnitStatType statType, int value)
    {
        var stat = combatant.Stats.SingleOrDefault(x => x.Type == statType);

        if (stat is null)
        {
            return value;
        }

        var d = Math.Min(value, stat.Value.Current);
        stat.Value.Consume(d);

        var remains = value - d;

        return remains;
    }
}