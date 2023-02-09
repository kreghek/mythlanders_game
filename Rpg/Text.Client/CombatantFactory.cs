using Core.Combats;

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
            new IEffect[]
            {
                new DamageEffect(new ClosestInLineTargetSelector(), new InstantaneousEffectImposer(),
                    Range<int>.CreateMono(2))
            })
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
            new IEffect[]
            {
                new DamageEffect(new ClosestInLineTargetSelector(), new InstantaneousEffectImposer(), Range<int>.CreateMono(1))

            })
        );
        var monster = new Combatant(monsterSequence) { Sid = "Digital wolf", IsPlayerControlled = false };

        return new[] { new FormationSlot(0, 1) { Combatant = monster } };
    }
}