using Core.Combats;

namespace Text.Client;

internal static class CombatantFactory
{
    public static IReadOnlyCollection<FormationSlot> CreateHeroes()
    {
        var swordsmanHero = new SwordsmanFartory();

        return new[]
        {
            new FormationSlot(0, 1)
            {
                Combatant = swordsmanHero.Create("Berimir")
            }
        };
    }

    public static IReadOnlyCollection<FormationSlot> CreateMonsters()
    {
        var chaserFactory = new ThiefChaserFactory();
        var wolfFactory = new DigitalWolfFactory();

        return new[]
        {
            new FormationSlot(0, 1)
            {
                Combatant = chaserFactory.Create("Chaser")
            },
            new FormationSlot(1, 2)
            {
                Combatant = chaserFactory.Create("Guard Chaser")
            },
            new FormationSlot(0, 2)
            {
                Combatant = wolfFactory.Create("Evil Digital wolf")
            }
        };
    }
}