using Core.Dices;

namespace Core.Combats;

public sealed class DamageEffect: IEffect
{
    private readonly ITargetSelector _selector;
    private readonly IEffectImposer _imposer;

    public DamageEffect(ITargetSelector selector, IEffectImposer imposer, Range<int> damage)
    {
        _selector = selector;
        _imposer = imposer;
        Damage = damage;
    }

    public ITargetSelector Selector => _selector;
    public IEffectImposer Imposer => _imposer;

    public Range<int> Damage { get; }

    public void Influence(Combatant target, IEffectCombatContext context)
    {
        var rolledDamage = context.Dice.Roll(Damage.Min, Damage.Max);

        var damageRemains = TakeStat(target, UnitStatType.ShieldPoints, rolledDamage);

        if (damageRemains > 0)
        {
            TakeStat(target, UnitStatType.HitPoints, damageRemains);
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
}