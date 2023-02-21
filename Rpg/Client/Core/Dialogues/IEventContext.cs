using Client.Core.Dialogues;

namespace Rpg.Client.Core.Dialogues
{
    internal interface IEventContext
    {
        DialogueEvent CurrentDualogueEvent { get; }
        void AddNewCharacter(Unit unit);
        void AddNewGlobalEvent(IGlobeEvent globalEvent);
        void AddStoryPoint(string storyPointSid);
        void StartCombat(string sid);
        void UnlockLocation(LocationSid locationSid);
    }
}