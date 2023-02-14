using Core.Combats.Effects;
using Core.Dices;

namespace Core.Combats;

public sealed class DamageEffectInstance : IEffectInstance
{
    private readonly DamageEffect _damageEffect;
    private IList<IEffectModifier> _modifiers;

    private Range<IStatValue> _damage;
    public DamageEffectInstance(DamageEffect damageEffect)
    {
        _damageEffect = damageEffect;

        _damage = new Range<IStatValue>(new StatValue(damageEffect.Damage.Min), new StatValue(damageEffect.Damage.Max));

        _modifiers = new List<IEffectModifier>();
    }

    public ITargetSelector Selector => _damageEffect.Selector;

    public void Influence(Combatant target, IEffectCombatContext context)
    {
        var rolledDamage = context.Dice.Roll(_damage.Min.ActualMax, _damage.Max.ActualMax);

        var absorbedDamage =
            Math.Max(rolledDamage - target.Stats.Single(x => x.Type == UnitStatType.Defense).Value.Current, 0);

        var damageRemains = TakeStat(target, UnitStatType.ShieldPoints, absorbedDamage);

        context.NotifyCombatantDamaged(target, UnitStatType.ShieldPoints, absorbedDamage - damageRemains);

        if (_damageEffect.DamageType == DamageType.ShieldsOnly) return;

        if (damageRemains > 0)
        {
            TakeStat(target, UnitStatType.HitPoints, damageRemains);
            context.NotifyCombatantDamaged(target, UnitStatType.HitPoints, damageRemains);
        }
    }

    private static int TakeStat(Combatant combatant, UnitStatType statType, int value)
    {
        var stat = combatant.Stats.SingleOrDefault(x => x.Type == statType);

        if (stat is null) return value;

        var d = Math.Min(value, stat.Value.Current);
        stat.Value.Consume(d);

        var remains = value - d;

        return remains;
    }

    public void AddModifier(IUnitStatModifier modifier)
    {
        _damage.Min.AddModifier(modifier);
    }

    public void RemoveModifier(IUnitStatModifier modifier)
    {
        _damage.Max.RemoveModifier(modifier);
    }
}