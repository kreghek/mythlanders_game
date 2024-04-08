using System.Collections.Generic;
using System.Linq;

using Client.Assets.StoryPointJobs;

using CombatDicesTeam.Combats;

namespace Client.Core;

public class GameProgression
{
    private readonly IList<GameProgressionEntry> _entries;

    public GameProgression()
    {
        _entries = new List<GameProgressionEntry>();

        Current = new GameProgressionTransition(new[]
            {
                new GameProgressionTrigger(new Job(
                    new JobScheme(JobScopeCatalog.Global, JobTypeCatalog.CompleteCampaigns, new JobGoalValue(1)),
                    string.Empty, string.Empty, string.Empty))
            },
            new[]
            {
                new GameProgressionEntry("CommandCenterAvailable"),
                new GameProgressionEntry("campaignMapAvailable")
            },
            Singleton<NullGameProgressionTransition>.Instance);
    }

    public GameProgressionTransition Current { get; }

    public IReadOnlyCollection<GameProgressionEntry> Entries => _entries.ToArray();

    public bool HasEntry(string value)
    {
        return Entries.SingleOrDefault(x => x.Value == value) is not null;
    }
}