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
            var freeSlots = _globe.Player.Party.GetFreeSlots();
            if (freeSlots.Any())
            {
                var selectedFreeSlot = freeSlots.First();
                _globe.Player.MoveToParty(unit, selectedFreeSlot.Index);
            }
            else
            {
                var units = new List<Unit>();
                units.AddRange(_globe.Player.Pool.Units);
                units.Add(unit);

                _globe.Player.Pool.Units = units;
            }
        }
    }
}