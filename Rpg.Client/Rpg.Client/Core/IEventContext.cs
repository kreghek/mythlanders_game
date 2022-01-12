namespace Rpg.Client.Core
{
    internal interface IEventContext
    {
        void AddNewCharacter(Unit unit);
        void AddNewGlobalEvent(IGlobeEvent globalEvent);
    }
}