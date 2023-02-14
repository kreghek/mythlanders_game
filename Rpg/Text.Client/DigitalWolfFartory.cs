using Core.Combats;
using Core.Combats.Effects;
using Core.Combats.TargetSelectors;

namespace Text.Client;

public class DigitalWolfFactory
{
    public Combatant Create(string sid)
    {
        var monsterSequence = new CombatMovementSequence();
        monsterSequence.Items.Add(new CombatMovement("Wolf teeth",
                new CombatMovementCost(1),
                CombatMovementEffectConfig.Create(
                    new IEffect[]
                    {
                        new DamageEffect(
                            new ClosestInLineTargetSelector(),
                            DamageType.Normal,
                            Range<int>.CreateMono(3))
                    })
            )
        );

        monsterSequence.Items.Add(new CombatMovement("Veles protection",
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

        monsterSequence.Items.Add(new CombatMovement("Cyber claws",
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

        var monster = new Combatant(monsterSequence)
        {
            Sid = sid, IsPlayerControlled = false
        };

        return monster;
    }
}