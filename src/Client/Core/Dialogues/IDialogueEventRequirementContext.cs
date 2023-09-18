using System.Collections.Generic;

namespace Client.Core.Dialogues;

public interface IDialogueEventRequirementContext
{
    IReadOnlyCollection<UnitName> ActiveHeroesInParty { get; }
    IReadOnlyCollection<string> ActiveStories { get; }
    ILocationSid CurrentLocation { get; }
    IReadOnlyCollection<string> DialogueKeys { get; }
}