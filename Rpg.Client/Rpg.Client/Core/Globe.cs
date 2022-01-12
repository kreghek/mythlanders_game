using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Rpg.Client.Core
{
    internal sealed class Globe
    {
        private List<IGlobeEvent> _globeEvents;

        public Globe(IBiomeGenerator biomeGenerator)
        {
            // First variant of the names.
            /*
             * "Поле брани", "Дикое болото", "Черные топи", "Лес колдуна", "Нечистивая\nяма",
             * "Мыс страха", "Тропа\nпогибели", "Кладбише\nпроклятых", "Выжженая\nдеревня", "Холм тлена"
             */

            var biomes = biomeGenerator.Generate();

            Biomes = biomes;
            CurrentBiome = biomes.Single(x => x.IsStart);

            _globeEvents = new List<IGlobeEvent>();
        }

        public Combat? ActiveCombat { get; set; }

        public IReadOnlyCollection<Biome> Biomes { get; }

        public Biome? CurrentBiome { get; set; }

        public Event? CurrentEvent { get; internal set; }

        public EventNode? CurrentEventNode { get; set; }

        public bool IsNodeInitialied { get; set; }

        public Player? Player { get; set; }

        public IReadOnlyCollection<IGlobeEvent> GlobeEvents
        {
            get => _globeEvents;
        }

        public void UpdateNodes(IDice dice, IUnitSchemeCatalog unitSchemeCatalog, IEventCatalog eventCatalog)
        {
            // Reset all combat states.
            var biomes = Biomes.Where(x => x.IsAvailable).ToArray();
            foreach (var biom in biomes)
            {
                foreach (var node in biom.Nodes)
                {
                    node.CombatSequence = null;
                    node.AssignedEvent = null;
                }

                if (biom.IsComplete && biom.UnlockBiome is not null)
                {
                    var unlockedBiom = Biomes.Single(x => x.Type == biom.UnlockBiome);

                    unlockedBiom.IsAvailable = true;
                }
            }

            // Create new combats
            foreach (var biome in biomes)
            {
                var availableNodes = biome.Nodes.Where(x => x.IsAvailable).ToArray();
                Debug.Assert(availableNodes.Any(), "At least of one node expected to be available.");
                var nodesWithCombats = GetNodesWithCombats(biome, dice, availableNodes);

                var combatCounts = GetCombatSequnceLength(biome.Level);
                var combatLevelAdditionalList = new[] { 0, -1, 3 };
                var selectedNodeCombatCount = dice.RollFromList(combatCounts, 3).ToArray();
                var combatLevelAdditional = 0;

                var combatToTrainingIndex = dice.RollArrayIndex(nodesWithCombats);

                for (var locationIndex = 0; locationIndex < nodesWithCombats.Length; locationIndex++)
                {
                    var selectedNode = nodesWithCombats[locationIndex];
                    var targetCombatSenquenceLength = selectedNode.Item2 ? 1 : selectedNodeCombatCount[locationIndex];

                    var combatLevel = biome.Level + combatLevelAdditionalList[combatLevelAdditional];
                    var combatList = new List<CombatSource>();
                    for (var combatIndex = 0; combatIndex < targetCombatSenquenceLength; combatIndex++)
                    {
                        var units = MonsterGeneratorHelper
                            .CreateMonsters(selectedNode.Item1, dice, biome, combatLevel, unitSchemeCatalog).ToArray();

                        var combat = new CombatSource
                        {
                            Level = combatLevel,
                            EnemyGroup = new Group(),
                            IsTrainingOnly = combatToTrainingIndex == locationIndex &&
                                             biome.Nodes.Where(x => x.IsAvailable).Count() == 4,
                            IsBossLevel = selectedNode.Item2
                        };

                        for (var slotIndex = 0; slotIndex < units.Length; slotIndex++)
                        {
                            var unit = units[slotIndex];
                            combat.EnemyGroup.Slots[slotIndex].Unit = unit;
                        }

                        combatList.Add(combat);
                    }

                    var combatSequence = new CombatSequence
                    {
                        Combats = combatList
                    };

                    selectedNode.Item1.CombatSequence = combatSequence;

                    combatLevelAdditional++;
                }
            }

            // create dialogs of nodes with combat
            foreach (var biome in biomes)
            {
                var nodesWithCombat = biome.Nodes.Where(x => x.CombatSequence is not null).ToArray();

                AssignEventToNodesWithCombat(biome, dice, nodesWithCombat, eventCatalog);
            }

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
            IEventCatalog eventCatalog)
        {
            var availableEvents = eventCatalog.Events
                .Where(x => (x.IsUnique && x.Counter == 0) || (!x.IsUnique))
                .Where(x => (x.Biome is not null && x.Biome == biome.Type) || (x.Biome is null))
                .Where(x => (x.RequiredBiomeLevel is not null && x.RequiredBiomeLevel <= biome.Level) ||
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

        private static int[] GetCombatSequnceLength(int level)
        {
            return level switch
            {
                0 or 1 => new[] { 1, 1, 1 },
                2 => new[] { 1, 1, 1, 3 },
                > 3 and <= 4 => new[] { 1, 1, 1, 3, 3 },
                > 5 and <= 7 => new[] { 1, 3, 3, 3, 5 },
                > 8 and <= 10 => new[] { 3, 3, 3, 5, 5 },
                > 10 => new[] { 3, 5, 5 },
                _ => new[] { 1, 1, 1, 1, 1, 1, 3, 3, 3, 5, 5 }
            };
        }

        private static (GlobeNode, bool)[] GetNodesWithCombats(Biome biome, IDice dice, GlobeNode[] availableNodes)
        {
            const int COMBAT_UNDER_ATTACK_COUNT = 3;
            const GlobeNodeSid BOSS_LOCATION_SID = GlobeNodeSid.Castle;

            var nodeList = new List<(GlobeNode, bool)>(3);
            var bossLocation = availableNodes.SingleOrDefault(x => x.Sid == BOSS_LOCATION_SID);
            int targetCount;
            if (biome.Level >= 10 && bossLocation is not null && !biome.IsComplete)
            {
                nodeList.Add(new(bossLocation, true));
                targetCount = Math.Min(availableNodes.Length, COMBAT_UNDER_ATTACK_COUNT - 1);
            }
            else
            {
                targetCount = Math.Min(availableNodes.Length, COMBAT_UNDER_ATTACK_COUNT);
            }

            var regularLocations = dice.RollFromList(availableNodes, targetCount);
            foreach (var location in regularLocations)
            {
                nodeList.Add(new(location, false));
            }

            return nodeList.ToArray();
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

        public event EventHandler? Updated;

        public void Update(IDice dice, IUnitSchemeCatalog unitSchemeCatalog, IEventCatalog eventCatalog)
        {
            UpdateGlobeEvents();
            UpdateNodes(dice, unitSchemeCatalog, eventCatalog);
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

        public void AddGlobalEvent(IGlobeEvent globalEvent)
        {
            _globeEvents.Add(globalEvent);
        }
    }
}