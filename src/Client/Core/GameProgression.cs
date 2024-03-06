using System;
using System.Collections.Generic;
using System.Linq;

using Client.Assets;
using Client.Assets.Catalogs.CampaignGeneration;
using Client.Assets.StageItems;
using Client.Assets.StoryPointJobs;
using Client.Core.CampaignEffects;
using Client.Core.Campaigns;

using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.CombatantStatuses;
using CombatDicesTeam.Graphs;
using CombatDicesTeam.Graphs.Generation.TemplateBased;

using Core.PropDrop;

namespace Client.Core;

internal class ScenarioCampaigns
{
    public HeroCampaign GetCampaign(string sid, Player player)
    {
        if (sid == "tutorial")
        {
            var locationSid = LocationSids.Thicket;
            
            var graph = new DirectedGraph<ICampaignStageItem>();
            var combatSequence = new CombatSequence
            {
                Combats = new[]
                {
                    new CombatSource(new[]
                    {
                        new PerkMonsterCombatantPrefab(new MonsterCombatantPrefab("ambushdrone", 0, new FieldCoords(0,0)), ArraySegment<ICombatantStatusFactory>.Empty),
                        new PerkMonsterCombatantPrefab(new MonsterCombatantPrefab("ambushdrone", 1, new FieldCoords(1,0)), ArraySegment<ICombatantStatusFactory>.Empty)
                    }, new CombatReward(Array.Empty<IDropTableScheme>()))
                }
            };
            
            
            graph.AddNode(new GraphNode<ICampaignStageItem>(new CombatStageItem(locationSid, combatSequence)));

            return new HeroCampaign(new[]
                {
                    (player.Heroes.Units.Single(x => x.ClassSid == "swordsman"), new FieldCoords(0, 0))
                }, new HeroCampaignLocation(LocationSids.Thicket, graph), ArraySegment<ICampaignEffect>.Empty,
                ArraySegment<ICampaignEffect>.Empty, 0);
        }

        throw new ArgumentException();
    }
}

public class GameProgression
{
    private readonly IList<GameProgressionEntry> _entries;
    
    public GameProgressionTransition Current { get; }

    public GameProgression()
    {
        _entries = new List<GameProgressionEntry>();

        Current = new GameProgressionTransition(new[]
            {
                new GameProgressionTrigger(new Job(
                    new JobScheme(JobScopeCatalog.Global, JobTypeCatalog.CompleteCampaigns, new JobGoalValue(1)),
                    String.Empty, String.Empty, String.Empty))
            },
            new[]
            {
                new GameProgressionEntry("CommandCenterAvailable"),
                new GameProgressionEntry("campaignMapAvailable")
            },
            Singleton<NullGameProgressionTransition>.Instance);
    }

    public IReadOnlyCollection<GameProgressionEntry> Entries => _entries.ToArray();

    public bool HasEntry(string value)
    {
        return Entries.SingleOrDefault(x => x.Value == value) is not null;
    }
}

public sealed record GameProgressionEntry(string Value);

public record GameProgressionTrigger(IJob BaseJob)
{
}

public interface IGameProgressionTransition
{
    IEnumerable<GameProgressionTrigger> Triggers { get; }
    IEnumerable<GameProgressionEntry> Entries { get; }
    IGameProgressionTransition Next { get;}
}

public class NullGameProgressionTransition : IGameProgressionTransition
{
    public IEnumerable<GameProgressionTrigger> Triggers => ArraySegment<GameProgressionTrigger>.Empty;
    public IEnumerable<GameProgressionEntry> Entries => ArraySegment<GameProgressionEntry>.Empty;
    public IGameProgressionTransition Next => Singleton<NullGameProgressionTransition>.Instance;
}

public record GameProgressionTransition(IEnumerable<GameProgressionTrigger> Triggers, IEnumerable<GameProgressionEntry> Entries, IGameProgressionTransition Next) : IGameProgressionTransition;