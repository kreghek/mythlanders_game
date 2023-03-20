using Core.Combats;
using Core.Combats.CombatantEffectLifetimes;
using Core.Combats.Effects;
using Core.Combats.TargetSelectors;

namespace Text.Client;

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
        );

        list.Add(new CombatMovement("Double kopesh!",
                new CombatMovementCost(2),
                CombatMovementEffectConfig.Create(
                    new IEffect[]
                    {
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
        );

        list.Add(new CombatMovement("Chasing",
                new CombatMovementCost(1),
                CombatMovementEffectConfig.Create(
                    new IEffect[]
                    {
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
        );

        var rolledSequence = list.OrderBy(_ => Guid.NewGuid()).ToArray();

        var monsterSequence = new CombatMovementSequence();
        for (var i = 0; i < 2; i++)
            foreach (var movement in rolledSequence)
                monsterSequence.Items.Add(movement);

        var monster = new Combatant("Chaser", monsterSequence, combatActorBehaviour)
        {
            Sid = sid, IsPlayerControlled = false
        };

        return monster;
    }
}