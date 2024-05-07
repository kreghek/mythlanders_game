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
    private readonly IList<HeroCampaignState> _heroes;

    public HeroCampaign(IReadOnlyCollection<(HeroState, FieldCoords)> initHeroes, HeroCampaignLocation location,
        IReadOnlyCollection<ICampaignEffect> rewards,
        IReadOnlyCollection<ICampaignEffect> failurePenalties, int visualizationSeed)
    {
        _heroes = new List<HeroCampaignState>(CreateCampaignHeroes(initHeroes));
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
    public IReadOnlyCollection<HeroCampaignState> Heroes => _heroes.ToArray();
    public HeroCampaignLocation Location { get; }


    public IList<IGraphNode<ICampaignStageItem>> Path { get; }

    public int VisualizationSeed { get; }

    public void AddHero(HeroState heroState)
    {
        var slots = new[]
        {
            (0, 0),
            (1, 0),

            (0, 1),
            (1, 1),

            (0, 2),
            (1, 2)
        };

        var free = slots.Except(_heroes.Select(x => (x.FormationSlot.ColumnIndex, x.FormationSlot.LineIndex)));

        if (free.Any())
        {
            var f1 = free.First();

            _heroes.Add(new HeroCampaignState(heroState, new FormationSlot(f1.Item1, f1.Item2)));
        }
    }

    public void FailCampaign(Globe globe, IJobProgressResolver jobProgressResolver)
    {
        ApplyCampaignEffects(globe, ActualFailurePenalties);
    }

    public void WinCampaign(Globe globe, IJobProgressResolver jobProgressResolver)
    {
        ApplyCampaignEffects(globe, ActualRewards);

        CountCampaignWon(globe, jobProgressResolver);
    }

    internal void CompleteCurrentStage(Globe globe, IJobProgressResolver jobProgressResolver)
    {
        var progress = new CompleteStageJobProgress();
        var executables = globe.GetCurrentJobExecutables();

        foreach (var job in executables)
        {
            jobProgressResolver.ApplyProgress(progress, job);
        }
    }

    private void ApplyCampaignEffects(Globe globe, IReadOnlyCollection<ICampaignEffect> effects)
    {
        foreach (var campaignEffect in effects)
        {
            campaignEffect.Apply(globe);
        }
    }

    private static void CountCampaignWon(Globe globe, IJobProgressResolver jobProgressResolver)
    {
        var completeCampaignProgress = new CampaignWonJobProgress();
        var executables = globe.GetCurrentJobExecutables();

        foreach (var job in executables)
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