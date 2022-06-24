namespace Rpg.Client.Core.Dialogues
{
    internal interface IEventContext
    {
        void AddNewCharacter(Unit unit);
        void AddNewGlobalEvent(IGlobeEvent globalEvent);
        void AddStoryPoint(string storyPointSid);
        void UnlockLocation(GlobeNodeSid locationSid);
    }
}