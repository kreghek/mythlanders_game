using System.Linq;

using Client.Assets.Catalogs.Dialogues;
using Client.Core;

namespace Client.Assets.DialogueEventRequirements;

internal sealed class LocationRequirement : IDialogueEventRequirement
{
    private readonly ILocationSid[] _locationSids;

    public LocationRequirement(params ILocationSid[] locationSids)
    {
        _locationSids = locationSids;
    }

    public bool IsApplicableFor(IDialogueEventRequirementContext context)
    {
        return _locationSids.Contains(context.CurrentLocation);
    }
}