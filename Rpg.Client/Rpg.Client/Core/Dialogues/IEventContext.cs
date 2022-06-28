namespace Rpg.Client.Core.Dialogues
{
    internal interface IEventContext
    {
        void AddNewCharacter(Unit unit);
        void AddNewGlobalEvent(IGlobeEvent globalEvent);
        void AddStoryPoint(string storyPointSid);
        void StartCombat(string sid);
        void UnlockLocation(GlobeNodeSid locationSid);
    }
}