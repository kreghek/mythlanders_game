using System.Collections.Generic;
using System.Linq;

using Client.Core;

using Core.Combats;

namespace Client.GameScreens.Combat.CombatDebugElements;

internal static class CombatantFactory
{
    public static IReadOnlyCollection<FormationSlot> CreateHeroes(ICombatActorBehaviour combatActorBehaviour, Player player)
    {
        var swordsmanHeroFactory = new SwordsmanFactory();
        var amazonHeroFactory = new AmazonFactory();
        var partisanHeroFactory = new PartisanFactory();

        var swordsmanHeroHitpointsStat = player.Heroes.Single(x => x.ClassSid == "swordsman").HitPoints;

        return new[]
        {
            new FormationSlot(0, 1)
            {
                Combatant = swordsmanHeroFactory.Create("Berimir", combatActorBehaviour, swordsmanHeroHitpointsStat)
            },
            new FormationSlot(1, 0)
            {
                Combatant = amazonHeroFactory.Create("Diana", combatActorBehaviour)
            },
            new FormationSlot(1, 2)
            {
                Combatant = partisanHeroFactory.Create("Deaf (the)", combatActorBehaviour)
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
                Combatant = chaserFactory.Create("Chaser", combatActorBehaviour, 0)
            },
            new FormationSlot(1, 2)
            {
                Combatant = chaserFactory.Create("Guard Chaser", combatActorBehaviour, 1)
            },
            new FormationSlot(0, 2)
            {
                Combatant = wolfFactory.Create("Evil Digital wolf", combatActorBehaviour)
            }
        };
    }
}