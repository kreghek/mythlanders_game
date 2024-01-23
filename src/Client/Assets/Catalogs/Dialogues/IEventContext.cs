using System.Collections.Generic;

using Client.Core;
using Client.Core.Heroes;

namespace Client.Assets.Catalogs.Dialogues;

internal interface IEventContext
{
    DialogueEvent CurrentDialogueEvent { get; }

    IReadOnlyCollection<string> CurrentHeroes { get; }
    void AddNewHero(string heroSid);
    void AddNewGlobalEvent(IGlobeEvent globalEvent);
    void AddStoryPoint(string storyPointSid);
    void StartCombat(string sid);
    void UnlockLocation(ILocationSid locationSid);
}