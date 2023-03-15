using System.Collections.Generic;

using Core.Combats;
using Core.Combats.Effects;
using Core.Combats.TargetSelectors;

namespace Client.GameScreens.Combat.CombatDebugElements;

public class AmazonFactory
{
    public Combatant Create(string sid, ICombatActorBehaviour combatActorBehaviour)
    {
        var movementPool = new List<CombatMovement>();

        movementPool.Add(new CombatMovement("Hunt",
            new CombatMovementCost(3),
            CombatMovementEffectConfig.Create(
                new IEffect[]
                {
                    new DamageEffect(
                            new ClosestInLineTargetSelector(),
                            DamageType.Normal,
                            Range<int>.CreateMono(4)),
                })
        )
        {
            Tags = CombatMovementTags.Attack
        });

        movementPool.Add(new CombatMovement("FinishWounded",
            new CombatMovementCost(2),
            CombatMovementEffectConfig.Create(
                new IEffect[]
                {
                    new DamageEffect(
                            new WeakestEnemyTargetSelector(),
                            DamageType.Normal,
                            Range<int>.CreateMono(4)),
                })
        )
        {
            Tags = CombatMovementTags.Attack
        });

        movementPool.Add(new CombatMovement("TrackerSavvy",
            new CombatMovementCost(1),
            CombatMovementEffectConfig.Create(
                new IEffect[]
                {
                    new ModifyEffectsEffect(new SelfTargetSelector(), 1),
                })
        ));

        movementPool.Add(new CombatMovement("JustHitBoarWithKnife",
            new CombatMovementCost(2),
            CombatMovementEffectConfig.Create(
                new IEffect[]
                {
                    new DamageEffect(
                            new ClosestInLineTargetSelector(),
                            DamageType.Normal,
                            Range<int>.CreateMono(1)),
                })
        )
        {
            Tags = CombatMovementTags.Attack
        });

        movementPool.Add(new CombatMovement("BringBeastDown",
            new CombatMovementCost(3),
            CombatMovementEffectConfig.Create(
                new IEffect[]
                {
                    new DamageEffect(
                            new StrongestEnemyTargetSelector(),
                            DamageType.Normal,
                            Range<int>.CreateMono(4)),
                })
        )
        {
            Tags = CombatMovementTags.Attack
        });

        var heroSequence = new CombatMovementSequence();

        for (var i = 0; i < 2; i++)
            foreach (var movement in movementPool)
                heroSequence.Items.Add(movement);

        var hero = new Combatant("amazon", heroSequence, combatActorBehaviour)
        {
            Sid = sid, IsPlayerControlled = true
        };
        return hero;
    }
}