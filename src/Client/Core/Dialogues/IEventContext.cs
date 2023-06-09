﻿using System.Collections.Generic;

using Client.Core.Heroes;

namespace Client.Core.Dialogues;

internal interface IEventContext
{
    DialogueEvent CurrentDialogueEvent { get; }

    IReadOnlyCollection<string> CurrentHeroes { get; }
    void AddNewCharacter(Hero unit);
    void AddNewGlobalEvent(IGlobeEvent globalEvent);
    void AddStoryPoint(string storyPointSid);
    void StartCombat(string sid);
    void UnlockLocation(ILocationSid locationSid);
}