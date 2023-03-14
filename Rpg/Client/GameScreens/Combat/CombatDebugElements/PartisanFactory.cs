using System.Collections.Generic;

using Client.Assets.CombatMovements.Hero.Swordsman;

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

        movementPool.Add(new CombatMovement("Rise your fists!",
            new CombatMovementCost(1),
            CombatMovementEffectConfig.Create(
                new IEffect[]
                {
                    new ModifyEffectsEffect(new SelfTargetSelector(), 1)
                })
        ));

        movementPool.Add(new DieBySwordFactory().CreateMovement());

        movementPool.Add(new CombatMovement("I'm so strong",
                new CombatMovementCost(2),
                new CombatMovementEffectConfig(
                    new IEffect[]
                    {
                        new ChangeStatEffect(new SelfTargetSelector(),
                            UnitStatType.Defense,
                            3,
                            typeof(ToNextCombatantTurnEffectLifetime))
                    },
                    new IEffect[]
                    {
                        new ChangeStatEffect(new SelfTargetSelector(),
                            UnitStatType.Defense,
                            1,
                            typeof(ToEndOfCurrentRoundEffectLifetime))
                    })
            )
        {
            Tags = CombatMovementTags.AutoDefense
        }
        );

        movementPool.Add(new CombatMovement("Hit from shoulder",
                new CombatMovementCost(3),
                CombatMovementEffectConfig.Create(
                    new IEffect[]
                    {
                        new DamageEffect(
                            new ClosestInLineTargetSelector(),
                            DamageType.Normal,
                            Range<int>.CreateMono(3)),
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

        movementPool.Add(new CombatMovement("Look out!",
            new CombatMovementCost(2),
            new CombatMovementEffectConfig(
                new IEffect[]
                {
                    new ChangeStatEffect(new ClosestAllyInColumnTargetSelector(),
                        UnitStatType.Defense,
                        3,
                        typeof(ToNextCombatantTurnEffectLifetime)),
                    new ChangePositionEffect(
                        new SelfTargetSelector(),
                        ChangePositionEffectDirection.ToVanguard
                    )
                },
                new IEffect[]
                {
                    new ChangeStatEffect(new SelfTargetSelector(),
                        UnitStatType.Defense,
                        1,
                        typeof(ToEndOfCurrentRoundEffectLifetime))
                })
        )
        {
            Tags = CombatMovementTags.AutoDefense
        });

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