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

    public bool IsApplicableFor(IDialogueEventRequirementContext context)
    {
        return _locationSids.Contains(context.CurrentLocation);
    }
}