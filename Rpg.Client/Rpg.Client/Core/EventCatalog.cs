﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;

using Rpg.Client.Core.EventSerialization;

namespace Rpg.Client.Core
{
    internal class EventCatalog : IEventCatalog
    {
        private readonly Event[] _events;
        private readonly IUnitSchemeCatalog _unitSchemeCatalog;

        public EventCatalog(IUnitSchemeCatalog unitSchemeCatalog)
        {
            _unitSchemeCatalog = unitSchemeCatalog;

            var rm = PlotResources.ResourceManager;
            var serializedPlotString = rm.GetString("MainPlot");

            Debug.Assert(serializedPlotString is not null, "It is required to resources contain serialized plot.");

            var eventStorageModelList = JsonSerializer.Deserialize<EventStorageModel[]>(serializedPlotString);

            Debug.Assert(eventStorageModelList is not null, "Plot event required to be correctly serializable.");
            
            var events = CreateEvents(eventStorageModelList);

            AssignEventParents(events, eventStorageModelList);

            _events = events.ToArray();
        }

        private static void AssignEventParents(IReadOnlyCollection<Event> events, IEnumerable<EventStorageModel> eventStorageModelList)
        {
            foreach (var eventStorageModel in eventStorageModelList)
            {
                if (eventStorageModel.ParentSids is null)
                {
                    continue;
                }
                
                var childEvent = events.Single(x => x.Sid == eventStorageModel.Sid);
                childEvent.RequiredEventsCompleted = eventStorageModel.ParentSids;
            }
        }

        private IReadOnlyCollection<Event> CreateEvents(EventStorageModel[] eventStorageModelList)
        {
            var events = CreateEventsIterator(eventStorageModelList).ToArray();
            return events;
        }

        private IEnumerable<Event> CreateEventsIterator(EventStorageModel[] eventStorageModelList)
        {
            foreach (var eventStorageModel in eventStorageModelList)
            {
                var locationInfo = GetLocationInfo(eventStorageModel.Location);

                var beforeEventNode = EventCatalogHelper.BuildEventNode(eventStorageModel.BeforeCombatNode,
                    EventPosition.BeforeCombat,
                    eventStorageModel.BeforeCombatAftermath, _unitSchemeCatalog);
                var afterEventNode = EventCatalogHelper.BuildEventNode(eventStorageModel.AfterCombatNode,
                    EventPosition.AfterCombat,
                    eventStorageModel.AfterCombatAftermath, unitSchemeCatalog: _unitSchemeCatalog);

                // System marker used to load saved game. Read it as identifier.
                var systemMarker = GetSystemEventMarker(eventStorageModel);

                var isMainPlotEvent = IsMainPlotEvent(eventStorageModel);
                const string? START_EVENT_SID = "MainSlavic1";
                var isGameStartEvent = eventStorageModel.Sid == START_EVENT_SID;
                var plotEvent = new Event
                {
                    Sid = eventStorageModel.Sid,
                    Biome = locationInfo.Biome,
                    ApplicableOnlyFor = new[] { locationInfo.LocationSid },
                    IsUnique = isMainPlotEvent,
                    IsHighPriority = isMainPlotEvent,
                    Title = eventStorageModel.Name,
                    BeforeCombatStartNode = beforeEventNode,
                    AfterCombatStartNode = afterEventNode,
                    SystemMarker = systemMarker,
                    IsGameStart = isGameStartEvent,
                    GoalDescription = StringHelper.TempLineBreaking(eventStorageModel.GoalDescription)
                };

                yield return plotEvent;
            }
        }

        private static bool IsMainPlotEvent(EventStorageModel eventStorageModel)
        {
            return eventStorageModel.Sid.StartsWith("Main");
        }

        private static SystemEventMarker? GetSystemEventMarker(EventStorageModel eventStorageModel)
        {
            string? aftermath = eventStorageModel.BeforeCombatAftermath ?? eventStorageModel.AfterCombatAftermath;

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

        private static LocationInfo GetLocationInfo(string location)
        {
            var locationSid = Enum.Parse<GlobeNodeSid>(location);
            var biomeValue = (((int)locationSid) / 100) * 100;
            var biome = (BiomeType)biomeValue;
            return new LocationInfo { Biome = biome, LocationSid = locationSid };
        }

        public IEnumerable<Event> Events => _events;

        private sealed record LocationInfo
        {
            public BiomeType Biome { get; init; }
            public GlobeNodeSid LocationSid { get; init; }
        }
    }
}