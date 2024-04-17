using System.Collections.Generic;
using System.Linq;

using Client.Assets.StoryPointJobs;

using CombatDicesTeam.Combats;

namespace Client.Core;

public sealed class CurrentGameFeatures
{
    private readonly ICollection<GameFeature> _features;

    public CurrentGameFeatures()
    {
        _features = new HashSet<GameFeature>();
    }

    public IReadOnlyCollection<GameFeature> Features => _features.ToArray();

    public bool HasFeature(GameFeature feature)
    {
        return Features.SingleOrDefault(x => x.Equals(feature)) is not null;
    }

    public void AddFeature(GameFeature feature)
    {
        if (!HasFeature(feature))
        {
            _features.Add(feature);
        }
    }
}

public static class GameFeatures
{
    public static GameFeature Campaigns { get; } = new GameFeature(nameof(Campaigns));
    public static GameFeature CampaignMap { get; } = new GameFeature(nameof(CampaignMap));
    public static GameFeature SideQuests { get; } = new GameFeature(nameof(SideQuests));
}