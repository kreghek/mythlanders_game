using System;
using System.Collections.Generic;

using Rpg.Client.Core;
using Rpg.Client.Core.Modifiers;
using Rpg.Client.Core.SkillEffects;
using Rpg.Client.Core.Skills;
using Core.Dices;

namespace Client.Core.SkillEffects;

internal class HealEffect : InstantaneousEffectBase
{
    public HealEffect(ICombatUnit actor)
    {
        Actor = actor;
        SourceSupport = actor.Unit.Support;
    }

    public ICombatUnit Actor { get; set; }
    public override IEnumerable<EffectRule> DispelRules { get; } = new List<EffectRule>();
    public override IEnumerable<EffectRule> ImposeRules { get; } = new List<EffectRule>();
    public override IEnumerable<EffectRule> InfluenceRules { get; } = new List<EffectRule>();

    public float PowerMultiplier { get; init; } = 1f;

    public float Scatter { get; init; } = 0.1f;

    public int SourceSupport { get; init; }

    public MinMax<int> CalculateHeal()
    {
        var absoluteSupport = SourceSupport * PowerMultiplier;
        var min = absoluteSupport - Scatter * absoluteSupport;
        var max = absoluteSupport + Scatter * absoluteSupport;

        if (CombatContext is not null)
        {
            if (Target != null)
            {
                min = CombatContext.Combat.ModifiersProcessor.Modify(Target, min, ModifierType.TakenHeal);
                max = CombatContext.Combat.ModifiersProcessor.Modify(Target, max, ModifierType.TakenHeal);
            }
        }

        var absoluteMin = (int)Math.Round(min, MidpointRounding.AwayFromZero);
        var absoluteMax = (int)Math.Round(max, MidpointRounding.AwayFromZero);

        return new MinMax<int>
        {
            Min = Math.Max(absoluteMin, 0),
            Max = Math.Max(absoluteMin, absoluteMax)
        };
    }

    protected override void InfluenceAction()
    {
        var heal = CalculateHeal();
        var rolledValue = CombatContext.Combat.Dice.Roll(heal.Min, heal.Max);

        var accumulatedValue = rolledValue;
        foreach (var perk in Actor.Unit.Perks)
        {
            var modifiedDamage = perk.ModifyHeal(accumulatedValue, CombatContext.Combat.Dice);
            accumulatedValue = modifiedDamage;
        }

        Target.RestoreHitPoints(rolledValue);
    }
}