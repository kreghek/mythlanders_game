using System.Collections.Generic;

namespace Rpg.Client.Core
{
    internal sealed class EventContext : IEventContext
    {
        private readonly Globe _globe;

        public EventContext(Globe globe)
        {
            _globe = globe;
        }

        public void AddNewCharacter(Unit unit)
        {
            var units = new List<Unit>(_globe.Player.Group.Units);

            units.Add(unit);

            _globe.Player.Group.Units = units;
        }
    }
}