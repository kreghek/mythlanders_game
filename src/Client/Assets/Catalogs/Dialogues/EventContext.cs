using System;
using System.Collections.Generic;
using System.Linq;

using Client.Core;
using Client.Core.Heroes;

namespace Client.Assets.Catalogs.Dialogues;

internal sealed class EventContext : IEventContext
{
    private readonly Globe _globe;
    private readonly Player _player;
    private readonly IStoryPointCatalog _storyPointCatalog;

    public EventContext(Globe globe, IStoryPointCatalog storyPointCatalog, Player player,
        DialogueEvent currentDialogueEvent)
    {
        _globe = globe;
        _storyPointCatalog = storyPointCatalog;
        _player = player;

        CurrentDialogueEvent = currentDialogueEvent;

        CurrentHeroes = player.Heroes.Units.Select(x => x.ClassSid).ToArray();
    }

    public DialogueEvent CurrentDialogueEvent { get; }

    public void AddNewCharacter(Hero unit)
    {
        _globe.Player.Heroes.AddNewUnit(HeroState.Create(unit.UnitScheme.Name.ToString()));
    }

    public void AddNewGlobalEvent(IGlobeEvent globalEvent)
    {
        _globe.AddGlobalEvent(globalEvent);
    }

    public void AddStoryPoint(string storyPointSid)
    {
        var storyPoint = _storyPointCatalog.GetAll().Single(x => x.Sid == storyPointSid);
        _globe.AddActiveStoryPoint(storyPoint);
    }

    public void StartCombat(string sid)
    {
        throw new NotImplementedException();
    }

    public void UnlockLocation(ILocationSid locationSid)
    {
        //_globe.Biomes.SelectMany(x => x.Nodes).Single(x => x.Sid == locationSid).IsAvailable = true;
    }

    /// <inheritdoc />
    public IReadOnlyCollection<string> CurrentHeroes { get; }
}