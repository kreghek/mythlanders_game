using System.Collections.Generic;
using System.Linq;

using Client.Core.Dialogues;

using Rpg.Client.Core;

namespace Client;

internal class DialogueEventRequirementContext: IDialogueEventRequirementContext
{
    private readonly Globe _globe;

    public DialogueEventRequirementContext(Globe globe, LocationSid currentLocation)
    {
        _globe = globe;
        CurrentLocation = currentLocation;
    }

    public LocationSid CurrentLocation { get; }
    public IReadOnlyCollection<string> DialogueKeys => _globe.Player.StoryState.Keys;

    public IReadOnlyCollection<UnitName> ActiveHeroesInParty =>
        _globe.Player.Party.GetUnits().Select(x => x.UnitScheme.Name).ToArray();
}