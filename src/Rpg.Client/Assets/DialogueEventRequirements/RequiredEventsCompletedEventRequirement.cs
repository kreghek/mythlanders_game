using System.Linq;

using Rpg.Client.Core;
using Rpg.Client.Core.Dialogues;

namespace Rpg.Client.Assets.DialogueEventRequirements
{
    internal sealed class RequiredEventsCompletedEventRequirement : ITextEventRequirement
    {
        private readonly IEventCatalog _eventCatalog;
        private readonly string[] _requiredEvents;

        public RequiredEventsCompletedEventRequirement(IEventCatalog eventCatalog, string[] requiredEvents)
        {
            _eventCatalog = eventCatalog;
            _requiredEvents = requiredEvents;
        }

        public bool IsApplicableFor(Globe globe, GlobeNode targetNode)
        {
            foreach (var eventSid in _requiredEvents)
            {
                var eventInfo = _eventCatalog.Events.SingleOrDefault(x => x.Sid == eventSid);

                if (eventInfo?.Completed != true)
                {
                    return false;
                }
            }

            return true;
        }
    }
}