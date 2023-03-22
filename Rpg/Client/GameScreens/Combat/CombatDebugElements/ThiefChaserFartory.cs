using System.Collections.Generic;

using Core.Combats;
using Core.Combats.CombatantEffectLifetimes;
using Core.Combats.Effects;
using Core.Combats.TargetSelectors;

namespace Client.GameScreens.Combat.CombatDebugElements;

public class ThiefChaserFactory
{
    public Combatant Create(string sid, ICombatActorBehaviour combatActorBehaviour)
    {
        // ReSharper disable once UseObjectOrCollectionInitializer
        var list = new List<CombatMovement>();

        list.Add(new CombatMovement("Crossing hit",
                new CombatMovementCost(1),
                CombatMovementEffectConfig.Create(
                    new IEffect[]
                    {
                        new AdjustPositionEffect(new SelfTargetSelector()),
                        new DamageEffect(
                            new ClosestInLineTargetSelector(),
                            DamageType.Normal,
                            Range<int>.CreateMono(1)),
                        new DamageEffect(
                            new ClosestInLineTargetSelector(),
                            DamageType.Normal,
                            Range<int>.CreateMono(1)),
                        new PushToPositionEffect(
                            new SelfTargetSelector(),
                            ChangePositionEffectDirection.ToVanguard
                        )
                    })
            )
            {
                Tags = CombatMovementTags.Attack
            }
        );

        list.Add(new CombatMovement("Double kopesh!",
                new CombatMovementCost(2),
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
                            ChangePositionEffectDirection.ToVanguard
                        )
                    })
            )
            {
                Tags = CombatMovementTags.Attack
            }
        );

        list.Add(new CombatMovement("Chasing",
                new CombatMovementCost(1),
                CombatMovementEffectConfig.Create(
                    new IEffect[]
                    {
                        new AdjustPositionEffect(new SelfTargetSelector()),
                        new DamageEffect(
                            new ClosestInLineTargetSelector(),
                            DamageType.Normal,
                            Range<int>.CreateMono(1)),
                        new PushToPositionEffect(
                            new ClosestInLineTargetSelector(),
                            ChangePositionEffectDirection.ToVanguard),
                        new ChangeCurrentStatEffect(
                            new ClosestInLineTargetSelector(),
                            UnitStatType.Resolve,
                            Range<int>.CreateMono(-2))
                    })
            )
            {
                Tags = CombatMovementTags.Attack
            }
        );

        list.Add(new CombatMovement("Guardian promise",
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

        list.Add(new CombatMovement("Afterlife Whirlwind",
                new CombatMovementCost(1),
                CombatMovementEffectConfig.Create(
                    new IEffect[]
                    {
                        new AdjustPositionEffect(new SelfTargetSelector()),
                        new DamageEffect(
                            new AllVanguardTargetSelector(),
                            DamageType.Normal,
                            Range<int>.CreateMono(1)),
                        new PushToPositionEffect(
                            new SelfTargetSelector(),
                            ChangePositionEffectDirection.ToVanguard
                        )
                    })
            )
            {
                Tags = CombatMovementTags.Attack
            }
        );

        var monsterSequence = new CombatMovementSequence();
        for (var i = 0; i < 2; i++)
        {
            foreach (var movement in list)
            {
                monsterSequence.Items.Add(movement);
            }
        }

        var stats = new CombatantStatsConfig();
        stats.SetValue(UnitStatType.HitPoints, 5);
        stats.SetValue(UnitStatType.ShieldPoints, 5);
        stats.SetValue(UnitStatType.Resolve, 7);

        var monster = new Combatant("chaser", monsterSequence, stats, combatActorBehaviour)
        {
            Sid = sid, IsPlayerControlled = false
        };

        return monster;
    }
}