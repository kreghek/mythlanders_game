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
            CombatMovementEffectConfig.Create(
                new IEffect[]
                {
                    new DamageEffect(new ClosestInLineTargetSelector(), new InstantaneousEffectImposer(),
                        Range<int>.CreateMono(2))
                })
            )
        { 
            Tags = CombatMovementTags.Attack
        }
        );

        heroSequence.Items.Add(new CombatMovement("Im  so strong",
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

        var hero = new Combatant(heroSequence)
        {
            Sid = "Berimir", IsPlayerControlled = true
        };
        return hero;
    }

    public static IReadOnlyCollection<FormationSlot> CreateMonsters()
    {
        var monsterSequence = new CombatMovementSequence();
        monsterSequence.Items.Add(new CombatMovement("Cyber claws",
            CombatMovementEffectConfig.Create(
                new IEffect[]
                {
                    new DamageEffect(new ClosestInLineTargetSelector(), new InstantaneousEffectImposer(),
                        Range<int>.CreateMono(3))
                })
            )
        );

        monsterSequence.Items.Add(new CombatMovement("Veles protection",
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

        var monster = new Combatant(monsterSequence) { Sid = "Digital wolf", IsPlayerControlled = false };

        return new[] { new FormationSlot(0, 1) { Combatant = monster } };
    }
}