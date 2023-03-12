using Client.Core.Dialogues;
using Client.Core.Heroes;

namespace Rpg.Client.Core.Dialogues
{
    internal interface IEventContext
    {
        DialogueEvent CurrentDualogueEvent { get; }
        void AddNewCharacter(Hero unit);
        void AddNewGlobalEvent(IGlobeEvent globalEvent);
        void AddStoryPoint(string storyPointSid);
        void StartCombat(string sid);
        void UnlockLocation(LocationSid locationSid);
    }
}