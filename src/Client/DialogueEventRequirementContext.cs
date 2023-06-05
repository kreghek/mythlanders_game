using System.Collections.Generic;
using System.Linq;

using Client.Core;
using Client.Core.Dialogues;

using Rpg.Client.Core;

namespace Client;

internal class DialogueEventRequirementContext : IDialogueEventRequirementContext
{
    private readonly IEventCatalog _eventCatalog;
    private readonly Globe _globe;

    public DialogueEventRequirementContext(Globe globe, ILocationSid currentLocation, IEventCatalog eventCatalog)
    {
        _globe = globe;
        CurrentLocation = currentLocation;
        _eventCatalog = eventCatalog;
    }

    public ILocationSid CurrentLocation { get; }
    public IReadOnlyCollection<string> DialogueKeys => _globe.Player.StoryState.Keys;

    public IReadOnlyCollection<UnitName> ActiveHeroesInParty =>
        _globe.Player.Party.GetUnits().Select(x => x.UnitScheme.Name).ToArray();

    public IReadOnlyCollection<string> ActiveStories =>
        _eventCatalog.Events.Where(x => x.IsStarted).Select(x => x.Sid).ToArray();
}