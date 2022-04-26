using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Rpg.Client.Core
{
    internal sealed class Globe
    {
        private readonly IBiomeGenerator _biomeGenerator;
        private readonly List<IGlobeEvent> _globeEvents;

        public Globe(IBiomeGenerator biomeGenerator)
        {
            _biomeGenerator = biomeGenerator;
            // First variant of the names.
            /*
             * "Поле брани", "Дикое болото", "Черные топи", "Лес колдуна", "Нечистивая\nяма",
             * "Мыс страха", "Тропа\nпогибели", "Кладбише\nпроклятых", "Выжженая\nдеревня", "Холм тлена"
             */

            var biomes = biomeGenerator.Generate();

            Biomes = biomes;

            _globeEvents = new List<IGlobeEvent>();

            GlobeLevel = new GlobeLevel();
        }

        public GlobeLevel GlobeLevel { get; }

        public Combat? ActiveCombat { get; set; }

        public IReadOnlyCollection<Biome> Biomes { get; }

        public Event? CurrentEvent { get; internal set; }

        public EventNode? CurrentEventNode { get; set; }

        public IReadOnlyCollection<IGlobeEvent> GlobeEvents => _globeEvents;

        public bool IsNodeInitialied { get; set; }

        public Player? Player { get; set; }

        public void AddGlobalEvent(IGlobeEvent globalEvent)
        {
            _globeEvents.Add(globalEvent);
            globalEvent.Initialize(this);
        }

        public void Update(IDice dice, IEventCatalog eventCatalog)
        {
            UpdateGlobeEvents();
            UpdateNodes(dice, eventCatalog);
        }

        public void UpdateNodes(IDice dice, IEventCatalog eventCatalog)
        {
            var biomes = Biomes.ToArray();

            RefreshBiomeStates(biomes);

            _biomeGenerator.CreateCombatsInBiomeNodes(biomes, GlobeLevel);

            CreateEventsInBiomeNodes(dice, eventCatalog, biomes, GlobeLevel);

            Updated?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Goal:
        /// Do not pick used event when there are unused event in the catalog.
        /// Algorithm:
        /// 1. We know the counters of all events in the catalog. So calc min and max counter to split all the event into ranked
        /// groups.
        /// 2. Next start with a group with min rank. And select random event from this group.
        /// 3. We take next group when we can't take any event from current group.
        /// </summary>
        /// <param name="dice"></param>
        /// <param name="nodesWithCombat"></param>
        private static void AssignEventToNodesWithCombat(Biome biome, IDice dice, GlobeNode[] nodesWithCombat,
            IEventCatalog eventCatalog, GlobeLevel globeLevel)
        {
            var availableEvents = eventCatalog.Events
                .Where(x => (x.IsUnique && x.Counter == 0) || (!x.IsUnique))
                .Where(x => (x.Biome is not null && x.Biome == biome.Type) || (x.Biome is null))
                .Where(x => (x.RequiredBiomeLevel is not null && x.RequiredBiomeLevel <= globeLevel.Level) ||
                            (x.RequiredBiomeLevel is null))
                .Where(x => IsUnlocked(x, eventCatalog.Events));
            var availableEventList = availableEvents.ToList();

            foreach (var node in nodesWithCombat)
            {
                if (!availableEventList.Any())
                {
                    // There are no available events.
                    // But there are nodes to assign events.
                    // Just break attempts. We can do nothing.
                    break;
                }

                var nodeEvents = availableEventList.Where(x =>
                    (x.ApplicableOnlyFor is null) ||
                    (x.ApplicableOnlyFor is not null && x.ApplicableOnlyFor.Contains(node.Sid))).ToArray();
                if (nodeEvents.Any())
                {
                    var highPriorityEvent = nodeEvents.FirstOrDefault(x => x.IsHighPriority);
                    if (highPriorityEvent is not null)
                    {
                        node.AssignedEvent = highPriorityEvent;
                    }
                    else
                    {
                        var roll = dice.Roll(1, 10);
                        if (roll > 5)
                        {
                            var minCounter = nodeEvents.Min(x => x.Counter);
                            var currentRankEventList = nodeEvents.Where(x => x.Counter == minCounter).ToArray();
                            var rolledEvent = dice.RollFromList(currentRankEventList, 1).Single();

                            node.AssignedEvent = rolledEvent;
                        }
                    }

                    availableEventList.Remove(node.AssignedEvent);
                }
            }
        }

        private static void ClearNodeStates(Biome biome)
        {
            foreach (var node in biome.Nodes)
            {
                node.CombatSequence = null;
                node.AssignedEvent = null;
            }
        }


        private static void CreateEventsInBiomeNodes(IDice dice, IEventCatalog eventCatalog, Biome[] biomes, GlobeLevel globeLevel)
        {
            // create dialogs of nodes with combat
            foreach (var biome in biomes)
            {
                var nodesWithCombat = biome.Nodes.Where(x => x.CombatSequence is not null).ToArray();

                AssignEventToNodesWithCombat(biome, dice, nodesWithCombat, eventCatalog, globeLevel);
            }
        }

        private static bool IsUnlocked(Event testedEvent, IEnumerable<Event> events)
        {
            if (testedEvent.RequiredEventsCompleted is null)
            {
                return true;
            }

            var completedEvents = events.Where(x => x.Completed).ToArray();
            foreach (var eventSid in testedEvent.RequiredEventsCompleted)
            {
                if (eventSid is null)
                {
                    continue;
                }

                var foundCompletedEvent = completedEvents.Any(x => x.Title == eventSid);
                if (!foundCompletedEvent)
                {
                    return false;
                }
            }

            return true;
        }

        private void RefreshBiomeStates(IEnumerable<Biome> biomes)
        {
            foreach (var biome in biomes)
            {
                ClearNodeStates(biome);
            }
        }

        private void UpdateGlobeEvents()
        {
            var eventsSnapshot = _globeEvents.ToArray();
            foreach (var globeEvent in eventsSnapshot)
            {
                if (globeEvent.IsActive)
                {
                    globeEvent.Update();
                }
                else
                {
                    _globeEvents.Remove(globeEvent);
                }
            }

            foreach (var unit in Player.GetAll())
            {
                var unitEffectsSnapshot = unit.GlobalEffects.ToArray();
                foreach (var effect in unitEffectsSnapshot)
                {
                    if (!effect.Source.IsActive)
                    {
                        unit.RemoveGlobalEffect(effect);
                    }
                }
            }
        }

        public event EventHandler? Updated;
    }
}