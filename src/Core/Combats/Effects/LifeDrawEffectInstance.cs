using Core.Dices;

namespace Core.Combats.Effects;

public sealed class LifeDrawEffectInstance : EffectInstanceBase<LifeDrawEffect>
{
    public LifeDrawEffectInstance(LifeDrawEffect damageEffect) : base(damageEffect)
    {
        Damage = new Range<IStatValue>(new StatValue(damageEffect.Damage.Min), new StatValue(damageEffect.Damage.Max));
    }

    public Range<IStatValue> Damage { get; }

    public override void AddModifier(IUnitStatModifier modifier)
    {
        Damage.Min.AddModifier(modifier);
        Damage.Max.AddModifier(modifier);
    }

    public override void Influence(Combatant target, IEffectCombatContext context)
    {
        var rolledDamage = context.Dice.Roll(Damage.Min.ActualMax, Damage.Max.ActualMax);

        var absorbedDamage =
            Math.Max(rolledDamage - target.Stats.Single(x => x.Type == ICombatantStatType.Defense).Value.Current, 0);

        //var damageRemains = TakeStat(target, UnitStatType.ShieldPoints, absorbedDamage);

        var damageRemains = context.DamageCombatantStat(target, ICombatantStatType.ShieldPoints, absorbedDamage);

        if (damageRemains > 0)
        {
            //TakeStat(target, UnitStatType.HitPoints, damageRemains);
            var stealedHitPoints = context.DamageCombatantStat(target, ICombatantStatType.HitPoints, damageRemains);

            context.Actor.Stats.Single(x => x.Type == ICombatantStatType.HitPoints).Value.Restore(damageRemains);
        }
    }

    public override void RemoveModifier(IUnitStatModifier modifier)
    {
        Damage.Min.RemoveModifier(modifier);
        Damage.Max.RemoveModifier(modifier);
    }

    private static int TakeStat(Combatant combatant, ICombatantStatType statType, int value)
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