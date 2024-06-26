﻿using System;
using System.Collections.Generic;
using System.Linq;

using Client.Assets.StoryPointJobs;

using CombatDicesTeam.Dices;

namespace Client.Core;

internal sealed class Globe
{
    private readonly IList<IStoryPoint> _activeStoryPointsList;

    private readonly List<IGlobeEvent> _globeEvents;

    public Globe(Player player, CurrentGameFeatures progression)
    {
        _globeEvents = new List<IGlobeEvent>();
        _activeStoryPointsList = new List<IStoryPoint>();

        Player = player;
        Features = progression;
        // First variant of the names.
        /*
         * "Поле брани", "Дикое болото", "Черные топи", "Лес колдуна", "Нечистивая\nяма",
         * "Мыс страха", "Тропа\nпогибели", "Кладбише\nпроклятых", "Выжженая\nдеревня", "Холм тлена"
         */

        //var biomes = biomeGenerator.GenerateStartState();

        GlobeLevel = new GlobeLevel();
    }

    public IEnumerable<IStoryPoint> ActiveStoryPoints => _activeStoryPointsList;
    public CurrentGameFeatures Features { get; }

    public IReadOnlyCollection<IGlobeEvent> GlobeEvents => _globeEvents;

    public GlobeLevel GlobeLevel { get; }

    public Player Player { get; }

    public void AddActiveStoryPoint(IStoryPoint storyPoint)
    {
        storyPoint.Completed += StoryPoint_Completed;
        _activeStoryPointsList.Add(storyPoint);
    }

    public void AddGlobalEvent(IGlobeEvent globalEvent)
    {
        _globeEvents.Add(globalEvent);
        globalEvent.Start(this);
    }

    public IEnumerable<IJobExecutable> GetCurrentJobExecutables()
    {
        foreach (var storyPoint in ActiveStoryPoints.ToArray())
        {
            yield return storyPoint;
        }

        if (Player.Challenge is not null)
        {
            yield return Player.Challenge;
        }

        foreach (var globeEvent in _globeEvents)
        {
            yield return globeEvent;
        }
    }

    public void ResetCombatScopeJobsProgress()
    {
        foreach (var executable in GetCurrentJobExecutables())
        {
            if (executable.CurrentJobs is null)
            {
                continue;
            }

            foreach (var job in executable.CurrentJobs)
            {
                if (ReferenceEquals(job.Scheme.Scope, JobScopeCatalog.Combat))
                {
                    job.Progress = 0;
                }
            }
        }
    }

    public void Update(IDice dice, IEventCatalog eventCatalog)
    {
        UpdateGlobeEvents();
        UpdateStoryPoints();
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
            if (!globeEvent.ExpirationConditions.All(x => x.IsComplete))
            {
                continue;
            }

            globeEvent.Finish(this);
            _globeEvents.Remove(globeEvent);
        }
    }

    private void UpdateStoryPoints()
    {
        ResetCombatScopeJobsProgress();
    }
}