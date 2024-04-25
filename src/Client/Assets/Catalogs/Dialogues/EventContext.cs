using System;
using System.Collections.Generic;
using System.Linq;

using Client.Core;

namespace Client.Assets.Catalogs.Dialogues;

internal sealed class EventContext : IEventContext
{
    private readonly Globe _globe;
    private readonly IStoryPointCatalog _storyPointCatalog;

    public EventContext(Globe globe, IStoryPointCatalog storyPointCatalog,
        DialogueEvent currentDialogueEvent)
    {
        _globe = globe;
        _storyPointCatalog = storyPointCatalog;

        CurrentDialogueEvent = currentDialogueEvent;

        CurrentHeroes = _globe.Player.Heroes.Select(x => x.ClassSid).ToArray();
    }

    public DialogueEvent CurrentDialogueEvent { get; }

    public void AddNewHero(string heroSid)
    {
        _globe.Player.AddHero(HeroState.Create(heroSid));
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