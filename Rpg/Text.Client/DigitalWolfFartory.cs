using Core.Combats;
using Core.Combats.CombatantEffectLifetimes;
using Core.Combats.Effects;
using Core.Combats.TargetSelectors;

namespace Text.Client;

public class DigitalWolfFactory
{
    public Combatant Create(string sid)
    {
        // ReSharper disable once UseObjectOrCollectionInitializer
        var list = new List<CombatMovement>();

        list.Add(new CombatMovement("Wolf teeth",
                new CombatMovementCost(1),
                CombatMovementEffectConfig.Create(
                    new IEffect[]
                    {
                        new DamageEffect(
                            new ClosestInLineTargetSelector(),
                            DamageType.Normal,
                            Range<int>.CreateMono(3)),
                        new ChangePositionEffect(
                            new SelfTargetSelector(),
                            ChangePositionEffectDirection.ToVanguard)
                    })
            )
        );

        list.Add(new CombatMovement("Veles protection",
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

        list.Add(new CombatMovement("Cyber claws",
                new CombatMovementCost(1),
                CombatMovementEffectConfig.Create(
                    new IEffect[]
                    {
                        new DamageEffect(
                            new MostShieldChargedTargetSelector(),
                            DamageType.ShieldsOnly,
                            Range<int>.CreateMono(3))
                    })
            )
        );

        var monsterSequence = new CombatMovementSequence();
        for (var i = 0; i < 2; i++)
            foreach (var combatMovement in list)
                monsterSequence.Items.Add(combatMovement);

        var monster = new Combatant("Digital wolf", monsterSequence, new BotCombatActorBehaviour())
        {
            Sid = sid,
            IsPlayerControlled = false
        };

        return monster;
    }
}