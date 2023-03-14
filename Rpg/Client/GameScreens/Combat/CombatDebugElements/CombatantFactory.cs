using System.Collections.Generic;

using Core.Combats;

namespace Client.GameScreens.Combat.CombatDebugElements;

internal static class CombatantFactory
{
    public static IReadOnlyCollection<FormationSlot> CreateHeroes(ICombatActorBehaviour combatActorBehaviour)
    {
        var swordsmanHero = new SwordsmanFactory();
        var amazonHero = new AmazonFactory();
        var partisanHero = new PartisanFactory();

        return new[]
        {
            new FormationSlot(0, 1)
            {
                Combatant = swordsmanHero.Create("Berimir", combatActorBehaviour)
            },
            new FormationSlot(1, 0)
            {
                Combatant = amazonHero.Create("Diana", combatActorBehaviour)
            },
            new FormationSlot(1, 2)
            {
                Combatant = partisanHero.Create("Deaf (the)", combatActorBehaviour)
            }
        };
    }

    public static IReadOnlyCollection<FormationSlot> CreateMonsters(ICombatActorBehaviour combatActorBehaviour)
    {
        var chaserFactory = new ThiefChaserFactory();
        var wolfFactory = new DigitalWolfFactory();

        return new[]
        {
            new FormationSlot(0, 1)
            {
                Combatant = chaserFactory.Create("Chaser", combatActorBehaviour)
            },
            new FormationSlot(1, 2)
            {
                Combatant = chaserFactory.Create("Guard Chaser", combatActorBehaviour)
            },
            new FormationSlot(0, 2)
            {
                Combatant = wolfFactory.Create("Evil Digital wolf", combatActorBehaviour)
            }
        };
    }
}