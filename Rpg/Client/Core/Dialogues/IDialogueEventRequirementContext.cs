using System.Collections.Generic;

using Rpg.Client.Core;

namespace Client.Core.Dialogues;

internal interface IDialogueEventRequirementContext
{
    IReadOnlyCollection<UnitName> ActiveHeroesInParty { get; }
    IReadOnlyCollection<string> ActiveStories { get; }
    LocationSid CurrentLocation { get; }
    IReadOnlyCollection<string> DialogueKeys { get; }
}