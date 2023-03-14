using System.Collections.Generic;

using Core.Combats;
using Core.Combats.CombatantEffectLifetimes;
using Core.Combats.Effects;
using Core.Combats.TargetSelectors;

namespace Client.GameScreens.Combat.CombatDebugElements;

public class PartisanFactory
{
    public Combatant Create(string sid, ICombatActorBehaviour combatActorBehaviour)
    {
        var movementPool = new List<CombatMovement>();

        movementPool.Add(new CombatMovement("InspirationalBreakthrough",
                new CombatMovementCost(2),
                CombatMovementEffectConfig.Create(
                    new IEffect[]
                    {
                        new DamageEffect(
                            new ClosestInLineTargetSelector(),
                            DamageType.Normal,
                            Range<int>.CreateMono(1)),
                        new ChangePositionEffect(
                            new SelfTargetSelector(),
                            ChangePositionEffectDirection.ToVanguard
                        )
                    })
            )
        {
            Tags = CombatMovementTags.Attack
        }
        );

        movementPool.Add(new CombatMovement("Sabotage",
                new CombatMovementCost(3),
                CombatMovementEffectConfig.Create(
                    new IEffect[]
                    {
                        new DamageEffect(
                            new WeakestEnemyTargetSelector(),
                            DamageType.Normal,
                            Range<int>.CreateMono(3)),
                        new ChangePositionEffect(
                            new SelfTargetSelector(),
                            ChangePositionEffectDirection.ToRearguard
                        )
                    })
            )
        );

        movementPool.Add(new CombatMovement("SurpriseManeuver",
                new CombatMovementCost(3),
                CombatMovementEffectConfig.Create(
                    new IEffect[]
                    {
                        new ChangePositionEffect(
                            new StrongestClosestAllyTargetSelector(),
                            ChangePositionEffectDirection.ToVanguard
                        ),
                        new ChangeStatEffect(
                            new StrongestClosestAllyTargetSelector(),
                            UnitStatType.Defense,
                            2,
                            typeof(ToNextCombatantTurnEffectLifetime)),
                        new ChangeStatEffect(
                            new SelfTargetSelector(),
                            UnitStatType.Defense,
                            2,
                            typeof(ToNextCombatantTurnEffectLifetime))
                    })
            )
        );

        movementPool.Add(new CombatMovement("BlankShot",
                new CombatMovementCost(1),
                CombatMovementEffectConfig.Create(
                    new IEffect[]
                    {
                        new DamageEffect(
                            new ClosestInLineTargetSelector(),
                            DamageType.Normal,
                            Range<int>.CreateMono(2)),
                        new ChangePositionEffect(
                            new SelfTargetSelector(),
                            ChangePositionEffectDirection.ToVanguard
                        )
                    })
            )
        {
            Tags = CombatMovementTags.Attack
        }
        );

        movementPool.Add(new CombatMovement("OldGoodBrawl",
                new CombatMovementCost(2),
                CombatMovementEffectConfig.Create(
                    new IEffect[]
                    {
                        new DamageEffect(
                            new ClosestInLineTargetSelector(),
                            DamageType.Normal,
                            Range<int>.CreateMono(2)),
                        new ChangePositionEffect(
                            new SelfTargetSelector(),
                            ChangePositionEffectDirection.ToVanguard
                        )
                    })
            )
        {
            Tags = CombatMovementTags.Attack
        }
        );


        var heroSequence = new CombatMovementSequence();

        for (var i = 0; i < 2; i++)
            foreach (var movement in movementPool)
                heroSequence.Items.Add(movement);

        var hero = new Combatant("partisan", heroSequence, combatActorBehaviour)
        {
            Sid = sid, IsPlayerControlled = true
        };
        return hero;
    }
}