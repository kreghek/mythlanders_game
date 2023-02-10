using Core.Dices;

namespace Core.Combats.Effects;

public sealed class DamageEffect: IEffect
{
    public DamageEffect(ITargetSelector selector, IEffectImposer imposer, DamageType damageType, Range<int> damage)
    {
        Selector = selector;
        Imposer = imposer;
        DamageType = damageType;
        Damage = damage;
    }

    public ITargetSelector Selector { get; }

    public IEffectImposer Imposer { get; }
    public DamageType DamageType { get; }
    public Range<int> Damage { get; }

    public void Influence(Combatant target, IEffectCombatContext context)
    {
        var rolledDamage = context.Dice.Roll(Damage.Min, Damage.Max);

        var absorbedDamage = Math.Max(rolledDamage - target.Stats.Single(x => x.Type == UnitStatType.Defense).Value.Current, 0);

        var damageRemains = TakeStat(target, UnitStatType.ShieldPoints, absorbedDamage);

        context.NotifyCombatantDamaged(target, UnitStatType.ShieldPoints, absorbedDamage - damageRemains);

        if (DamageType == DamageType.ShieldsOnly)
        {
            return;
        }

        if (damageRemains > 0)
        {
            TakeStat(target, UnitStatType.HitPoints, damageRemains);
            context.NotifyCombatantDamaged(target, UnitStatType.HitPoints, damageRemains);
        }
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

    public void Dispel(Combatant target)
    {
        // Do nothing
    }
}