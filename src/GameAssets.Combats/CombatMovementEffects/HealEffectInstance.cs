using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.Effects;
using CombatDicesTeam.Dices;
using CombatDicesTeam.GenericRanges;

using GameAssets.Combats;

namespace Core.Combats.Effects;

public sealed class HealEffectInstance : EffectInstanceBase<HealEffect>
{
    public HealEffectInstance(HealEffect healEffect) : base(healEffect)
    {
        Heal = new GenericRange<IStatValue>(new StatValue(healEffect.Heal.Min), new StatValue(healEffect.Heal.Max));
    }

    public GenericRange<IStatValue> Heal { get; }

    public override void AddModifier(IStatModifier modifier)
    {
        Heal.Min.AddModifier(modifier);
        Heal.Max.AddModifier(modifier);
    }

    public override void Influence(ICombatant target, IStatusCombatContext context)
    {
        var rolledHeal = context.Dice.Roll(Heal.Min.ActualMax, Heal.Max.ActualMax);

        context.RestoreCombatantStat(target, CombatantStatTypes.HitPoints, rolledHeal);
    }

    public override void RemoveModifier(IStatModifier modifier)
    {
        Heal.Min.RemoveModifier(modifier);
        Heal.Max.RemoveModifier(modifier);
    }
}