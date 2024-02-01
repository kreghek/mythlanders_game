using System.Collections.Generic;

using Client.Core;

namespace Client.Assets.Catalogs.Dialogues;

internal interface IEventContext
{
    DialogueEvent CurrentDialogueEvent { get; }

    IReadOnlyCollection<string> CurrentHeroes { get; }
    void AddNewGlobalEvent(IGlobeEvent globalEvent);
    void AddNewHero(string heroSid);
    void AddStoryPoint(string storyPointSid);
    void StartCombat(string sid);
    void UnlockLocation(ILocationSid locationSid);
}