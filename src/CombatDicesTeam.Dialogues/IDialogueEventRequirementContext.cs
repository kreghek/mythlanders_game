using System.Collections.Generic;

namespace Client.Core.Dialogues;

internal interface IDialogueEventRequirementContext
{
    IReadOnlyCollection<UnitName> ActiveHeroesInParty { get; }
    IReadOnlyCollection<string> ActiveStories { get; }
    ILocationSid CurrentLocation { get; }
    IReadOnlyCollection<string> DialogueKeys { get; }
}