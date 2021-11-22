using System.Collections.Generic;
using System.Linq;

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
            var units = new List<Unit>();
            if (_globe.Player.Party.Units.Count() < 3)
            {
                units.AddRange(_globe.Player.Party.Units);

                _globe.Player.Party.Units = units;
            }
            else
            {
                units.AddRange(_globe.Player.Pool.Units);

                _globe.Player.Pool.Units = units;
            }

            units.Add(unit);
        }
    }
}