using System.Collections.Generic;

using Rpg.Client.Core;

namespace Client.Core.Dialogues;

internal interface IDialogueEventRequirementContext
{
    LocationSid CurrentLocation { get; }
    IReadOnlyCollection<string> DialogueKeys { get; }
    IReadOnlyCollection<UnitName> ActiveHeroesInParty { get; }
    IReadOnlyCollection<string> ActiveStories { get; }
}