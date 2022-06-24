using System.Collections.Generic;
using System.Linq;

namespace Rpg.Client.Core.Dialogues
{
    internal sealed class EventContext : IEventContext
    {
        private readonly Globe _globe;
        private readonly IStoryPointCatalog _storyPointCatalog;

        public EventContext(Globe globe, IStoryPointCatalog storyPointCatalog)
        {
            _globe = globe;
            _storyPointCatalog = storyPointCatalog;
        }

        public void AddNewCharacter(Unit unit)
        {
            var freeSlots = _globe.Player.Party.GetFreeSlots().ToArray();
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

        public void AddNewGlobalEvent(IGlobeEvent globalEvent)
        {
            _globe.AddGlobalEvent(globalEvent);
        }

        public void AddStoryPoint(string storyPointSid)
        {
            var storyPoint = _storyPointCatalog.GetAll().Single(x => x.Sid == storyPointSid);
            _globe.AddActiveStoryPoint(storyPoint);
        }

        public void UnlockLocation(GlobeNodeSid locationSid)
        {
            _globe.Biomes.SelectMany(x => x.Nodes).Single(x => x.Sid == locationSid).IsAvailable = true;
        }
    }
}