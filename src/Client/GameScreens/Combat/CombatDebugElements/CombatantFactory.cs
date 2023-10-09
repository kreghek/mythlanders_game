using System.Collections.Generic;
using System.Linq;

using Client.Core;
using Client.GameScreens.Combat.CombatDebugElements.Heroes;
using Client.GameScreens.Combat.CombatDebugElements.Monsters.Black;
using Client.GameScreens.Combat.CombatDebugElements.Monsters.Egyptian;
using Client.GameScreens.Combat.CombatDebugElements.Monsters.Greek;
using Client.GameScreens.Combat.CombatDebugElements.Monsters.Slavic;

using CombatDicesTeam.Combats;

using GameAssets.Combats;

namespace Client.GameScreens.Combat.CombatDebugElements;

internal class CombatantFactory
{
    private static readonly IDictionary<string, IHeroCombatantFactory> _heroFactories =
        new Dictionary<string, IHeroCombatantFactory>
        {
            { "swordsman", new SwordsmanCombatantFactory() },
            { "amazon", new AmazonCombatantFactory() },
            { "partisan", new PartisanCombatantFactory() },
            { "robber", new RobberCombatantFactory() },
            { "monk", new MonkCombatantFactory() },
            { "guardian", new MonkCombatantFactory() }
        };

    private static readonly IDictionary<string, IMonsterCombatantFactory> _monsterFactories =
        new Dictionary<string, IMonsterCombatantFactory>
        {
            { "digitalwolf", new DigitalWolfCombatantFactory() },
            { "corruptedbear", new CorruptedBearCombatantFactory() },
            { "wisp", new WispCombatantFactory() },
            { "chaser", new ChaserCombatantFactory() },
            { "aspid", new AspidCombatantFactory() },
            { "volkolakwarrior", new VolkolakCombatantFactory() },
            { "agressor", new AgressorCombatantFactory() },
            { "ambushdrone", new AmbushDroneCombatantFactory() },
            { "automataur", new AutomataurCombatantFactory() }
        };

    public static IReadOnlyCollection<FormationSlot> CreateHeroes(ICombatActorBehaviour combatActorBehaviour,
        Player player)
    {
        var formationSlots = player.Heroes.Where(x => x.HitPoints.Current > 0).Select(hero =>
            new FormationSlot(hero.FormationPosition.ColumentIndex, hero.FormationPosition.LineIndex)
            {
                Combatant = _heroFactories[hero.ClassSid].Create(hero.ClassSid, combatActorBehaviour, hero.HitPoints)
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

    private static TestamentCombatant CreateMonsterCombatant(
        ICombatActorBehaviour combatActorBehaviour,
        MonsterCombatantPrefab monsterCombatantPrefab)
    {
        return _monsterFactories[monsterCombatantPrefab.ClassSid].Create(monsterCombatantPrefab.ClassSid,
            combatActorBehaviour, monsterCombatantPrefab.Variation);
    }
}