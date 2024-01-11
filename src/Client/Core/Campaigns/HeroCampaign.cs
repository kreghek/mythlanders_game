using System.Collections.Generic;
using System.Linq;

using Client.Assets.StoryPointJobs;
using Client.Core.CampaignEffects;

using CombatDicesTeam.Combats;
using CombatDicesTeam.Graphs;

namespace Client.Core.Campaigns;

/// <summary>
/// Mutable state of a campaign exploring.
/// </summary>
internal sealed class HeroCampaign
{
    public HeroCampaign(IReadOnlyCollection<(HeroState, FieldCoords)> initHeroes, HeroCampaignLocation location,
        IReadOnlyCollection<ICampaignEffect> rewards,
        IReadOnlyCollection<ICampaignEffect> failurePenalties, int visualizationSeed)
    {
        Heroes = CreateCampaignHeroes(initHeroes);
        Location = location;

        ActualRewards = rewards;
        ActualFailurePenalties = failurePenalties;

        VisualizationSeed = visualizationSeed;
        Path = new List<IGraphNode<ICampaignStageItem>>();
    }

    /// <summary>
    /// Effect which will apply if heroes fail campaign.
    /// Can be modified during campaign.
    /// </summary>
    public IReadOnlyCollection<ICampaignEffect> ActualFailurePenalties { get; }

    /// <summary>
    /// Effect which will apply if heroes win campaign.
    /// Can be modified during campaign.
    /// </summary>
    //TODO Add modifiers of effects
    public IReadOnlyCollection<ICampaignEffect> ActualRewards { get; }

    public IGraphNode<ICampaignStageItem>? CurrentStage { get; set; }
    public IReadOnlyCollection<HeroCampaignState> Heroes { get; }

    public HeroCampaignLocation Location { get; }


    public IList<IGraphNode<ICampaignStageItem>> Path { get; }

    public int VisualizationSeed { get; }

    public void FailCampaign(Globe globe, IJobProgressResolver jobProgressResolver)
    {
        ApplyCampaignEffects(globe, ActualFailurePenalties);
    }

    public void WinCampaign(Globe globe, IJobProgressResolver jobProgressResolver)
    {
        ApplyCampaignEffects(globe, ActualRewards);

        CountCampaignCompleteInActiveStoryPoints(globe, jobProgressResolver);
    }

    internal void CompleteCurrentStage()
    {
        //TODO Count stage complete job
        //throw new NotImplementedException();
    }

    private void ApplyCampaignEffects(Globe globe, IReadOnlyCollection<ICampaignEffect> effects)
    {
        foreach (var campaignEffect in effects)
        {
            campaignEffect.Apply(globe);
        }
    }

    private static void CountCampaignCompleteInActiveStoryPoints(Globe globe, IJobProgressResolver jobProgressResolver)
    {
        var completeCampaignProgress = new CampaignCompleteJobProgress();
        var currentJobs = globe.ActiveStoryPoints.ToArray();

        foreach (var job in currentJobs)
        {
            jobProgressResolver.ApplyProgress(completeCampaignProgress, job);
        }
    }

    private IReadOnlyCollection<HeroCampaignState> CreateCampaignHeroes(
        IEnumerable<(HeroState, FieldCoords)> heroes)
    {
        return heroes.Select(hero =>
                new HeroCampaignState(hero.Item1, new FormationSlot(hero.Item2.ColumentIndex, hero.Item2.LineIndex)))
            .ToList();
    }
}