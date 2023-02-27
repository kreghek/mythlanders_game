using System.Linq;

using Client.Core.Dialogues;

using Rpg.Client.Core;

namespace Client.Assets.DialogueEventRequirements;

internal sealed class LocationRequirement : IDialogueEventRequirement
{
    private readonly LocationSid[] _locationSids;

    public LocationRequirement(params LocationSid[] locationSids)
    {
        _locationSids = locationSids;
    }

    public bool IsApplicableFor(Globe globe, LocationSid targetLocation)
    {
        return _locationSids.Contains(targetLocation);
    }
}

internal sealed class StoryKeyRequirement: IDialogueEventRequirement
{
    private readonly string[] _requiredKeys;

    public StoryKeyRequirement(params string[] keys)
    {
        _requiredKeys = keys;
    }

    public bool IsApplicableFor(Globe globe, LocationSid targetLocation)
    {
        var activeKeys = globe.Player.StoryState.Keys;
        return _requiredKeys.All(x => activeKeys.Contains(x));
    }
}