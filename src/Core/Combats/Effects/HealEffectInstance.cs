using Core.Dices;

namespace Core.Combats.Effects;

public sealed class HealEffectInstance : EffectInstanceBase<HealEffect>
{
    public HealEffectInstance(HealEffect healEffect) : base(healEffect)
    {
        Heal = new Range<IStatValue>(new StatValue(healEffect.Heal.Min), new StatValue(healEffect.Heal.Max));
    }

    public Range<IStatValue> Heal { get; }

    public override void AddModifier(IUnitStatModifier modifier)
    {
        Heal.Min.AddModifier(modifier);
        Heal.Max.AddModifier(modifier);
    }

    public override void Influence(Combatant target, IStatusCombatContext context)
    {
        var rolledHeal = context.Dice.Roll(Heal.Min.ActualMax, Heal.Max.ActualMax);

        context.RestoreCombatantStat(target, CombatantStatTypes.HitPoints, rolledHeal);
    }

    public override void RemoveModifier(IUnitStatModifier modifier)
    {
        Heal.Min.RemoveModifier(modifier);
        Heal.Max.RemoveModifier(modifier);
    }
}