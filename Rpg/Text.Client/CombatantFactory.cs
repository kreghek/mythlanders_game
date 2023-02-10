using Core.Combats;
using Core.Combats.Effects;

namespace Text.Client;

internal static class CombatantFactory
{
    public static IReadOnlyCollection<FormationSlot> CreateHeroes()
    {
        var swordsmanHero = CreateSwordsman();

        return new[] { new FormationSlot(0, 1) { Combatant = swordsmanHero } };
    }

    private static Combatant CreateSwordsman()
    {
        var heroSequence = new CombatMovementSequence();

        heroSequence.Items.Add(new CombatMovement("Die by sword!",
            new CombatMovementCost(2),
            CombatMovementEffectConfig.Create(
                new IEffect[]
                {
                    new DamageEffect(
                        new ClosestInLineTargetSelector(), 
                        new InstantaneousEffectImposer(),
                        DamageType.Normal,
                        Range<int>.CreateMono(2))
                })
            )
        { 
            Tags = CombatMovementTags.Attack
        }
        );

        heroSequence.Items.Add(new CombatMovement("Im  so strong",
            new CombatMovementCost(2),
            new CombatMovementEffectConfig(
                new IEffect[]
                {
                    new DefenseEffect(new SelfTargetSelector(), new ToRoundEndEffectImposer(), new Range<int>(3, 3))
                },
                new IEffect[]
                {
                    new DefenseEffect(new SelfTargetSelector(), new ToRoundEndEffectImposer(), new Range<int>(1, 1))
                })
            )
        {
            Tags = CombatMovementTags.AutoDefense
        }
        );

        heroSequence.Items.Add(new CombatMovement("Hit from shoulder",
            new CombatMovementCost(3),
            CombatMovementEffectConfig.Create(
                new IEffect[]
                {
                    new DamageEffect(
                        new ClosestInLineTargetSelector(),
                        new InstantaneousEffectImposer(),
                        DamageType.Normal,
                        Range<int>.CreateMono(3))
                })
            )
        {
            Tags = CombatMovementTags.Attack
        }
        );

        var hero = new Combatant(heroSequence)
        {
            Sid = "Berimir",
            IsPlayerControlled = true
        };
        return hero;
    }

    public static IReadOnlyCollection<FormationSlot> CreateMonsters()
    {
        var monsterSequence = new CombatMovementSequence();
        monsterSequence.Items.Add(new CombatMovement("Wolf teeth",
            new CombatMovementCost(1),
            CombatMovementEffectConfig.Create(
                new IEffect[]
                {
                    new DamageEffect(
                        new ClosestInLineTargetSelector(),
                        new InstantaneousEffectImposer(),
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
                    new DefenseEffect(new SelfTargetSelector(), new ToRoundEndEffectImposer(), new Range<int>(3, 3))
                },
                new IEffect[]
                {
                    new DefenseEffect(new SelfTargetSelector(), new ToRoundEndEffectImposer(), new Range<int>(1, 1))
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
                        new InstantaneousEffectImposer(),
                        DamageType.ShieldsOnly,
                        Range<int>.CreateMono(3))
                })
            )
        );

        var monster = new Combatant(monsterSequence) { Sid = "Digital wolf", IsPlayerControlled = false };

        return new[] { new FormationSlot(0, 1) { Combatant = monster } };
    }
}