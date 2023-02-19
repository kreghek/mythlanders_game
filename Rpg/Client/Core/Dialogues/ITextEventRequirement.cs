using Rpg.Client.Core;

namespace Client.Core.Dialogues;

internal interface ITextEventRequirement
{
    bool IsApplicableFor(Globe globe, LocationSid targetLocation);
}