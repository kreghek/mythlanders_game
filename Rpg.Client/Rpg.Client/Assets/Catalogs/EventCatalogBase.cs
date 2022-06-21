using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;

using Rpg.Client.Core;
using Rpg.Client.Core.EventSerialization;

namespace Rpg.Client.Assets.Catalogs
{
    internal abstract class EventCatalogBase : IEventCatalog, IEventInitializer
    {
        private const int GOAL_TEXT_MAX_SYMBOL_COUNT = 60;

        private readonly IUnitSchemeCatalog _unitSchemeCatalog;
        private IEnumerable<Event> _events = null!;
        private bool _isInitialized;

        protected EventCatalogBase(IUnitSchemeCatalog unitSchemeCatalog)
        {
            _unitSchemeCatalog = unitSchemeCatalog;

            // To init use IEventInitializer.Init()
            _isInitialized = false;
        }

        protected abstract bool SplitIntoPages { get; }

        protected abstract string GetPlotResourceName();

        private static void AssignEventParents(IReadOnlyCollection<Event> events,
            IEnumerable<EventStorageModel> eventStorageModelList)
        {
            foreach (var eventStorageModel in eventStorageModelList)
            {
                if (eventStorageModel.ParentSids is null)
                {
                    continue;
                }

                var childEvent = events.Single(x => x.Sid == eventStorageModel.Sid);
                //childEvent.RequiredEventsCompleted = eventStorageModel.ParentSids;
            }
        }

        private IReadOnlyCollection<Event> CreateEvents(IEnumerable<EventStorageModel> eventStorageModelList)
        {
            var events = CreateEventsIterator(eventStorageModelList);
            return events.ToArray();
        }

        private IEnumerable<Event> CreateEventsIterator(IEnumerable<EventStorageModel> eventStorageModelList)
        {
            foreach (var eventStorageModel in eventStorageModelList)
            {
                var locationInfo = GetLocationInfo(eventStorageModel.Location);

                var beforeEventNode = EventCatalogHelper.BuildEventNode(eventStorageModel.BeforeCombatNode,
                    EventPosition.BeforeCombat,
                    eventStorageModel.BeforeCombatAftermath, _unitSchemeCatalog,
                    SplitIntoPages);
                var afterEventNode = EventCatalogHelper.BuildEventNode(eventStorageModel.AfterCombatNode,
                    EventPosition.AfterCombat,
                    eventStorageModel.AfterCombatAftermath, unitSchemeCatalog: _unitSchemeCatalog,
                    SplitIntoPages);

                // System marker used to load saved game. Read it as identifier.
                var systemMarker = GetSystemEventMarker(eventStorageModel);

                var isMainPlotEvent = IsMainPlotEvent(eventStorageModel);
                const string? START_EVENT_SID = "MainSlavic1";
                var isGameStartEvent = eventStorageModel.Sid == START_EVENT_SID;
                var plotEvent = new Event
                {
                    Sid = eventStorageModel.Sid,
                    //Biome = locationInfo.Biome,
                    //ApplicableOnlyFor = new[] { locationInfo.LocationSid },
                    //IsUnique = isMainPlotEvent,
                    //IsHighPriority = isMainPlotEvent,
                    //Title = eventStorageModel.Name,
                    //BeforeCombatStartNode = beforeEventNode,
                    //AfterCombatStartNode = afterEventNode,
                    //SystemMarker = systemMarker,
                    //IsGameStart = isGameStartEvent,
                    //GoalDescription = eventStorageModel.GoalDescription is not null
                    //    ? StringHelper.LineBreaking(eventStorageModel.GoalDescription,
                    //        GOAL_TEXT_MAX_SYMBOL_COUNT)
                    //    : null
                };

                yield return plotEvent;
            }
        }

        private static LocationInfo GetLocationInfo(string location)
        {
            var locationSid = Enum.Parse<GlobeNodeSid>(location);
            var biomeValue = (((int)locationSid) / 100) * 100;
            var biome = (BiomeType)biomeValue;
            return new LocationInfo { Biome = biome, LocationSid = locationSid };
        }

        private static SystemEventMarker? GetSystemEventMarker(EventStorageModel eventStorageModel)
        {
            var aftermath = eventStorageModel.BeforeCombatAftermath ?? eventStorageModel.AfterCombatAftermath;

            if (aftermath is null)
            {
                return null;
            }

            if (!Enum.TryParse<SystemEventMarker>(aftermath, out var systemEventMarker))
            {
                return null;
            }

            return systemEventMarker;
        }

        private static bool IsMainPlotEvent(EventStorageModel eventStorageModel)
        {
            return eventStorageModel.Sid.StartsWith("Main");
        }

        public IEnumerable<Event> Events
        {
            get
            {
                if (!_isInitialized)
                {
                    throw new InvalidOperationException("Init catalog first.");
                }

                return _events;
            }
            private set => _events = value;
        }

        public void Init()
        {
            var rm = PlotResources.ResourceManager;
            var resourceName = GetPlotResourceName();
            var serializedPlotString = rm.GetString(resourceName);

            Debug.Assert(serializedPlotString is not null, "It is required to resources contain serialized plot.");

            var eventStorageModelList = JsonSerializer.Deserialize<EventStorageModel[]>(serializedPlotString);

            Debug.Assert(eventStorageModelList is not null, "Plot event required to be correctly serializable.");

            var events = CreateEvents(eventStorageModelList);

            AssignEventParents(events, eventStorageModelList);

            Events = events.ToArray();

            _isInitialized = true;
        }

        public EventNode GetDialogRoot(string sid)
        {
            throw new NotImplementedException();
        }

        private sealed record LocationInfo
        {
            public BiomeType Biome { get; init; }
            public GlobeNodeSid LocationSid { get; init; }
        }
    }
}