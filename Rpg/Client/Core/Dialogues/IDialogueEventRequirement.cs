using Rpg.Client.Core;

namespace Client.Core.Dialogues;

internal interface IDialogueEventRequirement
{
    bool IsApplicableFor(Globe globe, LocationSid targetLocation);
}