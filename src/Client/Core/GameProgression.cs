using System;
using System.Collections.Generic;
using System.Linq;

using Client.Assets.StoryPointJobs;

using CombatDicesTeam.Combats;

namespace Client.Core;

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
                new GameProgressionEntry("MapAvailable")
            },
            Singleton<NullGameProgressionTransition>.Instance);
    }

    public IReadOnlyCollection<GameProgressionEntry> Entries => _entries.ToArray();
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