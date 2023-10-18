using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.Effects;
using CombatDicesTeam.Dices;
using CombatDicesTeam.GenericRanges;

using GameAssets.Combats;

namespace Core.Combats.Effects;

public sealed class LifeDrawEffectInstance : EffectInstanceBase<LifeDrawEffect>
{
    public LifeDrawEffectInstance(LifeDrawEffect damageEffect) : base(damageEffect)
    {
        Damage = new GenericRange<IStatValue>(new StatValue(damageEffect.Damage.Min),
            new StatValue(damageEffect.Damage.Max));
    }

    public GenericRange<IStatValue> Damage { get; }

    public override void AddModifier(IStatModifier modifier)
    {
        Damage.Min.AddModifier(modifier);
        Damage.Max.AddModifier(modifier);
    }

    public override void Influence(ICombatant target, IStatusCombatContext context)
    {
        var rolledDamage = context.Dice.Roll(Damage.Min.ActualMax, Damage.Max.ActualMax);

        var absorbedDamage =
            Math.Max(rolledDamage - target.Stats.Single(x => Equals(x.Type, CombatantStatTypes.Defense)).Value.Current,
                0);

        var damageRemains = context.DamageCombatantStat(target, CombatantStatTypes.ShieldPoints, absorbedDamage);

        if (damageRemains > 0)
        {
            //TakeStat(target, UnitStatType.HitPoints, damageRemains);
            var stolenHitPoints = context.DamageCombatantStat(target, CombatantStatTypes.HitPoints, damageRemains);

            context.Actor.Stats.Single(x => x.Type == CombatantStatTypes.HitPoints).Value.Restore(damageRemains);
        }
    }

    public override void RemoveModifier(IStatModifier modifier)
    {
        Damage.Min.RemoveModifier(modifier);
        Damage.Max.RemoveModifier(modifier);
    }
}