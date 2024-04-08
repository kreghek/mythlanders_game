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
            { "DigitalWolf", new DigitalWolfCombatantFactory() },
            { "CorruptedBear", new CorruptedBearCombatantFactory() },
            { "Wisp", new WispCombatantFactory() },
            { "Chaser", new ChaserCombatantFactory() },
            { "Aspid", new AspidCombatantFactory() },
            { "VolkolakWarrior", new VolkolakCombatantFactory() },
            { "Agressor", new AggressorCombatantFactory() },
            { "AmbushDrone", new AmbushDroneCombatantFactory() },
            { "Automataur", new AutomataurCombatantFactory() },
            { "PaintedSkin", new PaintedSkinCombatantFactory() }
        };

    public static IReadOnlyCollection<FormationSlot> CreateHeroes(ICombatActorBehaviour combatActorBehaviour,
        HeroCampaign campaign)
    {
        var aliveHeroes = campaign.Heroes.Where(x => x.HitPoints.Current > 0);
        var formationSlots = aliveHeroes.Select(heroState =>
            new FormationSlot(heroState.FormationSlot.ColumnIndex, heroState.FormationSlot.LineIndex)
            {
                Combatant = new MythlandersCombatant(heroState.ClassSid,
                    CreateCombatMovementSequence(heroState.AvailableMovements),
                    CreateStats(heroState.HitPoints, heroState.CombatStats),
                    combatActorBehaviour,
                    CreateInitialCombatStatuses(heroState))
                {
                    DebugSid = heroState.ClassSid,
                    IsPlayerControlled = true
                }
            }).ToArray();

        return formationSlots;
    }

    public static IReadOnlyCollection<FormationSlot> CreateMonsters(ICombatActorBehaviour combatActorBehaviour,
        IReadOnlyCollection<PerkMonsterCombatantPrefab> monsters)
    {
        var formation = new List<FormationSlot>();

        foreach (var monsterCombatantPrefab in monsters)
        {
            var formationSlot = new FormationSlot(monsterCombatantPrefab.TemplatePrefab.FormationInfo.ColumentIndex,
                monsterCombatantPrefab.TemplatePrefab.FormationInfo.LineIndex);

            var monsterCombatant = CreateMonsterCombatant(combatActorBehaviour, monsterCombatantPrefab);

            formationSlot.Combatant = monsterCombatant;

            formation.Add(formationSlot);
        }

        return formation;
    }

    private static CombatMovementSequence CreateCombatMovementSequence(
        IReadOnlyCollection<CombatMovement> availableMovements)
    {
        var sequence = new CombatMovementSequence();

        foreach (var move in availableMovements)
        {
            sequence.Items.Add(move);
        }

        return sequence;
    }

    private static IReadOnlyCollection<ICombatantStatusFactory> CreateInitialCombatStatuses(HeroCampaignState heroState)
    {
        return heroState.StartUpCombatStatuses;
    }

    private static MythlandersCombatant CreateMonsterCombatant(
        ICombatActorBehaviour combatActorBehaviour,
        PerkMonsterCombatantPrefab monsterCombatantPrefab)
    {
        var monsterClassSid = monsterCombatantPrefab.TemplatePrefab.ClassSid;
        var monsterCombatantFactory = _monsterFactories[monsterClassSid];
        var combatant = monsterCombatantFactory.Create(monsterClassSid,
            combatActorBehaviour, monsterCombatantPrefab.TemplatePrefab.Variation,
            monsterCombatantPrefab.Perks.Select(x => x.Status).ToArray());

        return combatant;
    }

    private static CombatantStatsConfig CreateStats(IStatValue hitPoints, IEnumerable<ICombatantStat> combatStats)
    {
        var stats = new CombatantStatsConfig();

        stats.SetValue(CombatantStatTypes.HitPoints, hitPoints);

        foreach (var stat in combatStats)
        {
            stats.SetValue(stat.Type, new StatValue(stat.Value.ActualMax));
        }

        return stats;
    }
}