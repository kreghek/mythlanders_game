using System;
using System.Collections.Generic;
using System.Linq;

using Client.Assets.Catalogs.Dialogues;
using Client.Core;

using Core.Props;

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

    public bool HasResource(PropScheme propScheme, int minimalAmount)
    {
        var resource = _globe.Player.Inventory.CalcActualItems().OfType<Resource>()
            .SingleOrDefault(x => x.Scheme.Sid == propScheme.Sid);
        return resource is not null && resource.Count >= minimalAmount;
    }

    public IReadOnlyCollection<UnitName> ActiveHeroesInParty =>
        _globe.Player.Heroes.Units.Select(x => Enum.Parse<UnitName>(x.ClassSid, true)).ToArray();

    public IReadOnlyCollection<string> ActiveStories =>
        _eventCatalog.Events.Where(x => x.IsStarted).Select(x => x.Sid).ToArray();
}