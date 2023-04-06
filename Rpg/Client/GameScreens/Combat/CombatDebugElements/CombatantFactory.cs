using System.Collections.Generic;
using System.Linq;

using Client.Core;

using Core.Combats;

namespace Client.GameScreens.Combat.CombatDebugElements;

internal class CombatantFactory
{
    private static readonly IDictionary<string, IHeroCombatantFactory> _factories =
        new Dictionary<string, IHeroCombatantFactory>
        {
            { "swordsman", new SwordsmanFactory() },
            { "amazon", new AmazonFactory() },
            { "partisan", new PartisanFactory() },
            { "robber", new RobberFactory() },
            { "monk", new MonkFactory() }
        };

    private static readonly IDictionary<string, IMonsterCombatantFactory> _monsterfactories =
        new Dictionary<string, IMonsterCombatantFactory>
        {
            { "digitalwolf", new DigitalWolfFactory() },
            { "chaser", new ThiefChaserFactory() },
            { "aspid", new AspidFactory() },
            { "volkolakwarrior", new VolkolakWarriorFactory() }
        };

    public static IReadOnlyCollection<FormationSlot> CreateHeroes(ICombatActorBehaviour combatActorBehaviour,
        Player player)
    {
        var formationSlots = player.Heroes.Select(hero =>
            new FormationSlot(hero.FormationPosition.ColumentIndex, hero.FormationPosition.LineIndex)
            {
                Combatant = _factories[hero.ClassSid].Create("", combatActorBehaviour, hero.HitPoints)
            }).ToArray();

        return formationSlots;
    }

    public static IReadOnlyCollection<FormationSlot> CreateMonsters(ICombatActorBehaviour combatActorBehaviour,
        IReadOnlyCollection<MonsterCombatantPrefab> monsters)
    {
        var formation = new List<FormationSlot>();

        foreach (var monsterCombatantPrefab in monsters)
        {
            var formationSlot = new FormationSlot(monsterCombatantPrefab.FormationInfo.ColumentIndex,
                monsterCombatantPrefab.FormationInfo.LineIndex);

            var monsterCombatant = CreateMonsterCombatant(combatActorBehaviour, monsterCombatantPrefab);

            formationSlot.Combatant = monsterCombatant;

            formation.Add(formationSlot);
        }

        return formation;
    }

    private static Combatant CreateMonsterCombatant(
        ICombatActorBehaviour combatActorBehaviour,
        MonsterCombatantPrefab monsterCombatantPrefab)
    {
        return _monsterfactories[monsterCombatantPrefab.ClassSid].Create(monsterCombatantPrefab.ClassSid,
            combatActorBehaviour, monsterCombatantPrefab.Variation);
    }
}