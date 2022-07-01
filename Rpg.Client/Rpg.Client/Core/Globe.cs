using System;
using System.Collections.Generic;
using System.Linq;

using Rpg.Client.Assets.StoryPointJobs;
using Rpg.Client.Core.Dialogues;

namespace Rpg.Client.Core
{
    internal sealed class Globe
    {
        private readonly IList<IStoryPoint> _activeStoryPointsList;
        private readonly IBiomeGenerator _biomeGenerator;
        private readonly List<IGlobeEvent> _globeEvents;

        public Globe(IBiomeGenerator biomeGenerator, Player player)
        {
            _globeEvents = new List<IGlobeEvent>();
            _activeStoryPointsList = new List<IStoryPoint>();

            _biomeGenerator = biomeGenerator;
            Player = player;
            // First variant of the names.
            /*
             * "Поле брани", "Дикое болото", "Черные топи", "Лес колдуна", "Нечистивая\nяма",
             * "Мыс страха", "Тропа\nпогибели", "Кладбише\nпроклятых", "Выжженая\nдеревня", "Холм тлена"
             */

            var biomes = biomeGenerator.GenerateStartState();

            Biomes = biomes;
            GlobeLevel = new GlobeLevel();
        }

        public IEnumerable<IStoryPoint> ActiveStoryPoints => _activeStoryPointsList;

        public IReadOnlyCollection<Biome> Biomes { get; }

        public IReadOnlyCollection<IGlobeEvent> GlobeEvents => _globeEvents;

        public GlobeLevel GlobeLevel { get; }

        public bool IsNodeInitialized { get; set; }

        public Player Player { get; }

        public void AddActiveStoryPoint(IStoryPoint storyPoint)
        {
            storyPoint.Completed += StoryPoint_Completed;
            _activeStoryPointsList.Add(storyPoint);
        }

        public void AddCombat(GlobeNode targetNode)
        {
            // var combatList = new List<CombatSource>();
            //
            // var combatSequence = new CombatSequence
            // {
            //     Combats = combatList
            // };
            //
            // targetNode.CombatSequence = combatSequence;
            //
            // Updated?.Invoke(this, EventArgs.Empty);
        }

        public void AddGlobalEvent(IGlobeEvent globalEvent)
        {
            _globeEvents.Add(globalEvent);
            globalEvent.Initialize(this);
        }

        public void AddMonster(CombatSource combatSource, Unit unit, int slotIndex)
        {
            combatSource.EnemyGroup.Slots[slotIndex].Unit = unit;
            Updated?.Invoke(this, EventArgs.Empty);
        }

        public void Update(IDice dice, IEventCatalog eventCatalog)
        {
            UpdateGlobeEvents();
            UpdateNodes(dice, eventCatalog);
            UpdateStoryPoints();
        }

        public void UpdateNodes(IDice dice, IEventCatalog eventCatalog)
        {
            var biomes = Biomes.ToArray();

            RefreshBiomeStates(biomes);

            _biomeGenerator.CreateCombatsInBiomeNodes(biomes, GlobeLevel);

            CreateDialoguesInBiomeNodes(dice, eventCatalog, biomes, GlobeLevel);

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
        private void AssignEventToNodesWithCombat(Biome biome, IDice dice, GlobeNode[] nodesWithCombat,
            IEventCatalog eventCatalog, GlobeLevel globeLevel)
        {
            var availableEvents = eventCatalog.Events
                .Where(x => (x.IsUnique && x.Counter == 0) || (!x.IsUnique))
                .Where(x => IsRequirementsPassed(nodesWithCombat, x));
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

                var nodeEvents = availableEventList
                    .Where(x => x.Requirements is null || x.Requirements.All(r => r.IsApplicableFor(this, node)))
                    .ToArray();
                if (nodeEvents.Any())
                {
                    var highPriorityEvent = nodeEvents.FirstOrDefault(x => x.Priority == TextEventPriority.High);
                    if (highPriorityEvent is not null)
                    {
                        node.AssignedEvent = highPriorityEvent;
                        availableEventList.Remove(node.AssignedEvent);
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
                            availableEventList.Remove(node.AssignedEvent);
                        }
                    }
                }
            }
        }

        private static void ClearNodeStates(Biome biome)
        {
            foreach (var node in biome.Nodes)
            {
                node.AssignedCombatSequence = null;
                node.AssignedEvent = null;
            }
        }


        private void CreateDialoguesInBiomeNodes(IDice dice, IEventCatalog eventCatalog, Biome[] biomes,
            GlobeLevel globeLevel)
        {
            // create dialogs of nodes with combat
            foreach (var biome in biomes)
            {
                var nodesWithCombat = biome.Nodes.Where(x => x.AssignedCombatSequence is not null).ToArray();

                AssignEventToNodesWithCombat(biome, dice, nodesWithCombat, eventCatalog, globeLevel);
            }
        }

        private bool IsRequirementsPassed(GlobeNode[] nodesWithCombat, Event x)
        {
            return x.Requirements is null ||
                   nodesWithCombat.Any(node => x.Requirements.All(r => r.IsApplicableFor(this, node)));
        }

        private static void RefreshBiomeStates(IEnumerable<Biome> biomes)
        {
            foreach (var biome in biomes)
            {
                ClearNodeStates(biome);
            }
        }

        private void ResetCombatScopeJobsProgress()
        {
            foreach (var storyPoint in ActiveStoryPoints)
            {
                if (storyPoint.CurrentJobs is null)
                {
                    continue;
                }

                foreach (var job in storyPoint.CurrentJobs)
                {
                    if (job.Scheme.Scope == JobScopeCatalog.Combat)
                    {
                        job.Progress = 0;
                    }
                }
            }
        }

        private void StoryPoint_Completed(object? sender, EventArgs e)
        {
            var storyPoint = sender as IStoryPoint;

            if (storyPoint is null)
            {
                throw new InvalidOperationException();
            }

            storyPoint.Completed -= StoryPoint_Completed;
            _activeStoryPointsList.Remove(storyPoint);
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

        private void UpdateStoryPoints()
        {
            ResetCombatScopeJobsProgress();
        }

        public event EventHandler? Updated;
    }
}