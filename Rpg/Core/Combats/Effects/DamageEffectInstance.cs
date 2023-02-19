using Core.Dices;

namespace Core.Combats.Effects;

public sealed class DamageEffectInstance : EffectInstanceBase<DamageEffect>
{

    public DamageEffectInstance(DamageEffect damageEffect) : base(damageEffect)
    {
        Damage = new Range<IStatValue>(new StatValue(damageEffect.Damage.Min), new StatValue(damageEffect.Damage.Max));
    }

    public Range<IStatValue> Damage { get; }

    public override void Influence(Combatant target, IEffectCombatContext context)
    {
        var rolledDamage = context.Dice.Roll(Damage.Min.ActualMax, Damage.Max.ActualMax);

        var absorbedDamage =
            Math.Max(rolledDamage - target.Stats.Single(x => x.Type == UnitStatType.Defense).Value.Current, 0);

        var damageRemains = TakeStat(target, UnitStatType.ShieldPoints, absorbedDamage);

        context.NotifyCombatantDamaged(target, UnitStatType.ShieldPoints, absorbedDamage - damageRemains);

        if (BaseEffect.DamageType == DamageType.ShieldsOnly) return;

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

    public override void AddModifier(IUnitStatModifier modifier)
    {
        Damage.Min.AddModifier(modifier);
        Damage.Max.AddModifier(modifier);
    }

    public override void RemoveModifier(IUnitStatModifier modifier)
    {
        Damage.Min.RemoveModifier(modifier);
        Damage.Max.RemoveModifier(modifier);
    }
}