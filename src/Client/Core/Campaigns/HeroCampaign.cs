using System;
using System.Collections.Generic;
using System.Linq;

using Client.Assets.StageItems;
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
        IReadOnlyCollection<ICampaignEffect> failurePenalties, int visualizationSeed)
    {
        Heroes = CreateCampaignHeroes(initHeroes);
        Location = location;

        ActualRewards = location.Stages.GetAllNodes().Select(x => x.Payload)
                            .OfType<IRewardCampaignStageItem>().FirstOrDefault()?.GetEstimateRewards(location) ??
                        ArraySegment<ICampaignEffect>.Empty;
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

    internal void CompleteCurrentStage()
    {
        //TODO Count stage complete job
        //throw new NotImplementedException();
    }

    private IReadOnlyCollection<HeroCampaignState> CreateCampaignHeroes(
        IEnumerable<(HeroState, FieldCoords)> heroes)
    {
        return heroes.Select(hero =>
                new HeroCampaignState(hero.Item1, new FormationSlot(hero.Item2.ColumentIndex, hero.Item2.LineIndex)))
            .ToList();
    }
}