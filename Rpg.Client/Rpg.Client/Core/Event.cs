using System.Collections.Generic;
using System.Linq;

namespace Rpg.Client.Core
{
    internal sealed class Event
    {
        public string? AfterCombatStartNodeSid { get; init; }
        public string? BeforeCombatStartNodeSid { get; init; }
        public bool Completed { get; set; }
        public int Counter { get; set; }
        public bool IsGameStart { get; init; }
        public bool IsUnique { get; init; }
        public TextEventPriority Priority { get; init; }
        public IReadOnlyCollection<ITextEventRequirement>? Requirements { get; init; }
        public string? Sid { get; init; }
    }

    internal interface ITextEventRequirement
    {
        bool IsApplicableFor(Globe globe, GlobeNode targetNode);
    }

    internal sealed class LocationEventRequirement : ITextEventRequirement
    {
        private readonly IReadOnlyCollection<GlobeNodeSid> _locationSids;

        public LocationEventRequirement(IReadOnlyCollection<GlobeNodeSid> locationSids)
        {
            _locationSids = locationSids;
        }

        public bool IsApplicableFor(Globe globe, GlobeNode targetNode)
        {
            return _locationSids.Contains(targetNode.Sid);
        }
    }

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

    internal sealed class RequiredGlobeProgressEventRequirement : ITextEventRequirement
    {
        private readonly int _progress;

        public RequiredGlobeProgressEventRequirement(int progress)
        {
            _progress = progress;
        }

        public bool IsApplicableFor(Globe globe, GlobeNode targetNode)
        {
            return globe.GlobeLevel.Level >= _progress;
        }
    }

    internal enum TextEventPriority
    {
        Normal,
        High
    }
}