using System;
using System.Collections.Generic;
using System.Linq;

using Client.Assets.StageItems;
using Client.Assets.StoryPointJobs;
using Client.Core;
using Client.Core.Campaigns;

using CombatDicesTeam.Dices;
using CombatDicesTeam.Graphs;
using CombatDicesTeam.Graphs.Generation.TemplateBased;

namespace Client.Assets.Catalogs.CampaignGeneration;

internal sealed class ChallengeCampaignStageTemplateFactory : ICampaignStageTemplateFactory
{
    private readonly CampaignStageTemplateServices _services;

    public ChallengeCampaignStageTemplateFactory(CampaignStageTemplateServices services)
    {
        _services = services;
    }

    private static ICampaignStageItem[] MapContextToCurrentStageItems(IGraphTemplateContext<ICampaignStageItem> context)
    {
        return context.CurrentWay.Select(x => x.Payload).ToArray();
    }

    public bool CanCreate(IReadOnlyList<ICampaignStageItem> currentStageItems)
    {
        return true;
    }

    public ICampaignStageItem Create(IReadOnlyList<ICampaignStageItem> currentStageItems)
    {
        var jobs = CreateChallengeJobs();
        return new ChallengeStageItem(jobs);
    }

    private IReadOnlyCollection<IJob> CreateChallengeJobs()
    {
        var count = _services.Dice.Roll(MIN_JOBS, MAX_JOBS);

        var jobList = new List<IJob>();

        for (int i = 0; i < count; i++)
        {
            var jobScheme = new JobScheme(JobScopeCatalog.Campaign, JobTypeCatalog.Defeats, new JobGoalValue(4));
            var job = new Job(jobScheme, string.Empty, String.Empty, String.Empty);
            jobList.Add(job);
        }

        return jobList;
    }

    private const int MAX_JOBS = 3;

    private const int MIN_JOBS = 1;

    /// <inheritdoc />
    public IGraphNode<ICampaignStageItem> Create(IGraphTemplateContext<ICampaignStageItem> context)
    {
        return new GraphNode<ICampaignStageItem>(Create(MapContextToCurrentStageItems(context)));
    }

    /// <inheritdoc />
    public bool CanCreate(IGraphTemplateContext<ICampaignStageItem> context)
    {
        return CanCreate(MapContextToCurrentStageItems(context));
    }
}