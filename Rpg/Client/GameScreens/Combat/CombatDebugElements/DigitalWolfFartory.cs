using System.Collections.Generic;

using Core.Combats;
using Core.Combats.CombatantEffectLifetimes;
using Core.Combats.Effects;
using Core.Combats.TargetSelectors;

namespace Client.GameScreens.Combat.CombatDebugElements;

public class DigitalWolfFactory
{
    public Combatant Create(string sid, ICombatActorBehaviour combatActorBehaviour)
    {
        // ReSharper disable once UseObjectOrCollectionInitializer
        var list = new List<CombatMovement>();

        list.Add(new CombatMovement("TeethOnNeck",
                new CombatMovementCost(1),
                CombatMovementEffectConfig.Create(
                    new IEffect[]
                    {
                        new AdjustPositionEffect(new SelfTargetSelector()),
                        new DamageEffect(
                            new ClosestInLineTargetSelector(),
                            DamageType.Normal,
                            Range<int>.CreateMono(3)),
                        new PushToPositionEffect(
                            new SelfTargetSelector(),
                            ChangePositionEffectDirection.ToVanguard)
                    })
            )
            {
                Tags = CombatMovementTags.Attack
            }
        );

        list.Add(new CombatMovement("VelesProtection",
                new CombatMovementCost(1),
                new CombatMovementEffectConfig(
                    new IEffect[]
                    {
                        new ChangeStatEffect(
                            new SelfTargetSelector(),
                            UnitStatType.Defense,
                            3,
                            typeof(ToNextCombatantTurnEffectLifetime))
                    },
                    new IEffect[]
                    {
                        new ChangeStatEffect(
                            new SelfTargetSelector(),
                            UnitStatType.Defense,
                            1,
                            typeof(ToEndOfCurrentRoundEffectLifetime))
                    })
            )
            {
                Tags = CombatMovementTags.AutoDefense
            }
        );

        list.Add(new CombatMovement("CyberClaws",
                new CombatMovementCost(1),
                CombatMovementEffectConfig.Create(
                    new IEffect[]
                    {
                        new AdjustPositionEffect(new SelfTargetSelector()),
                        new DamageEffect(
                            new MostShieldChargedTargetSelector(),
                            DamageType.ShieldsOnly,
                            Range<int>.CreateMono(3))
                    })
            )
            {
                Tags = CombatMovementTags.Attack
            }
        );

        var monsterSequence = new CombatMovementSequence();
        for (var i = 0; i < 3; i++)
        {
            foreach (var combatMovement in list)
            {
                monsterSequence.Items.Add(combatMovement);
            }
        }

        var monster = new Combatant("digitalwolf", monsterSequence, combatActorBehaviour)
        {
            Sid = sid, IsPlayerControlled = false
        };

        return monster;
    }
}