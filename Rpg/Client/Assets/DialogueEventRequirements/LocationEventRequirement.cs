using System.Linq;

using Client.Core;
using Client.Core.Dialogues;

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