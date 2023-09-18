using System.Collections.Generic;

using Client.Core;

namespace Client.Assets.Catalogs.Dialogues;

public interface IDialogueEventRequirementContext
{
    IReadOnlyCollection<UnitName> ActiveHeroesInParty { get; }
    IReadOnlyCollection<string> ActiveStories { get; }
    ILocationSid CurrentLocation { get; }
    IReadOnlyCollection<string> DialogueKeys { get; }
}