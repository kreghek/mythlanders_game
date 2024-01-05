using System;
using System.Collections.Generic;
using System.Linq;

using Client.Core;
using Client.Core.Campaigns;
using Client.GameScreens.Combat.CombatDebugElements.Monsters.Black;
using Client.GameScreens.Combat.CombatDebugElements.Monsters.Egyptian;
using Client.GameScreens.Combat.CombatDebugElements.Monsters.Greek;
using Client.GameScreens.Combat.CombatDebugElements.Monsters.Slavic;

using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.CombatantStatuses;

using GameAssets.Combats;

namespace Client.GameScreens.Combat.CombatDebugElements;

internal class CombatantFactory
{
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
        HeroCampaign campaign)
    {
        var aliveHeroes = campaign.Heroes.Where(x => x.HitPoints.Current > 0);
        var formationSlots = aliveHeroes.Select(heroState =>
            new FormationSlot(heroState.FormationSlot.ColumnIndex, heroState.FormationSlot.LineIndex)
            {
                Combatant = new TestamentCombatant(heroState.ClassSid,
                    CreateCombatMovementSequence(heroState.AvailableMovements),
                    CreateStats(heroState.HitPoints, heroState.CombatStats),
                    combatActorBehaviour,
                    CreateInitialCombatStatuses(heroState))
            }).ToArray();

        return formationSlots;
    }

    private static IReadOnlyCollection<ICombatantStatusFactory> CreateInitialCombatStatuses(HeroCampaignState heroState)
    {
        //TODO Use "global" status of hero from campaign
        return Array.Empty<ICombatantStatusFactory>();
    }

    private static CombatantStatsConfig CreateStats(IStatValue hitPoints, IEnumerable<ICombatantStat> combatStats)
    {
        var stats = new CombatantStatsConfig();

        stats.SetValue(CombatantStatTypes.HitPoints, hitPoints);

        foreach (var stat in combatStats)
        {
            stats.SetValue(stat.Type, stat.Value);
        }

        return stats;
    }

    private static CombatMovementSequence CreateCombatMovementSequence(IReadOnlyCollection<CombatMovement> availableMovements)
    {
        var sequence = new CombatMovementSequence();

        foreach (var move in availableMovements)
        {
            sequence.Items.Add(move);
        }

        return sequence;
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