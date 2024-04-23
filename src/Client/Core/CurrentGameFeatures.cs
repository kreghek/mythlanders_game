using System.Collections.Generic;
using System.Linq;

namespace Client.Core;

public sealed class CurrentGameFeatures
{
    private readonly ICollection<GameFeature> _features;

    public CurrentGameFeatures()
    {
        _features = new HashSet<GameFeature>();
    }

    public IReadOnlyCollection<GameFeature> Features => _features.ToArray();

    public void AddFeature(GameFeature feature)
    {
        if (!HasFeature(feature))
        {
            _features.Add(feature);
        }
    }

    public bool HasFeature(GameFeature feature)
    {
        return Features.SingleOrDefault(x => x.Equals(feature)) is not null;
    }
}