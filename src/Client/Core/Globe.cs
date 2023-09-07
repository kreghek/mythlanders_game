using System;
using System.Collections.Generic;

using Client.Assets.StoryPointJobs;

using CombatDicesTeam.Dices;

namespace Client.Core;

internal sealed class Globe
{
    private readonly IList<IStoryPoint> _activeStoryPointsList;

    //private readonly IBiomeGenerator _biomeGenerator;
    private readonly List<IGlobeEvent> _globeEvents;

    public Globe( /*IBiomeGenerator biomeGenerator,*/ Player player)
    {
        _globeEvents = new List<IGlobeEvent>();
        _activeStoryPointsList = new List<IStoryPoint>();

        //_biomeGenerator = biomeGenerator;
        Player = player;
        // First variant of the names.
        /*
         * "Поле брани", "Дикое болото", "Черные топи", "Лес колдуна", "Нечистивая\nяма",
         * "Мыс страха", "Тропа\nпогибели", "Кладбише\nпроклятых", "Выжженая\nдеревня", "Холм тлена"
         */

        //var biomes = biomeGenerator.GenerateStartState();

        GlobeLevel = new GlobeLevel();
    }

    public IEnumerable<IStoryPoint> ActiveStoryPoints => _activeStoryPointsList;


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

    public void Update(IDice dice, IEventCatalog eventCatalog)
    {
        UpdateGlobeEvents();
        UpdateNodes(dice, eventCatalog);
        UpdateStoryPoints();
    }

    public void UpdateNodes(IDice dice, IEventCatalog eventCatalog)
    {
        Updated?.Invoke(this, EventArgs.Empty);
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
    }

    private void UpdateStoryPoints()
    {
        ResetCombatScopeJobsProgress();
    }

    public event EventHandler? Updated;
}