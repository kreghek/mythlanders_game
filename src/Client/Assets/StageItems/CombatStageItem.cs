using System.Linq;

using Client.Core;
using Client.Core.Campaigns;
using Client.GameScreens.Combat;
using Client.ScreenManagement;

namespace Client.Assets.StageItems;

internal sealed class CombatStageItem : ICampaignStageItem
{
    private readonly ILocationSid _location;

    public CombatStageItem(ILocationSid location, CombatSequence combatSequence)
    {
        _location = location;
        CombatSequence = combatSequence;

        var combatSource = CombatSequence.Combats.First();

        var leaderPrefab = combatSource.Monsters.OrderByDescending(x => x.Perks.Count).First();
        var sumPts = combatSource.Monsters.Sum(x => x.Perks.Count + 1);

        Metadata = new CombatMetadata(leaderPrefab.TemplatePrefab, CalcDifficulty(sumPts));
    }

    public CombatMetadata Metadata { get; }

    internal CombatSequence CombatSequence { get; }

    private static CombatEstimateDifficulty CalcDifficulty(int sumPts)
    {
        switch (sumPts)
        {
            case > 4:
                return CombatEstimateDifficulty.Hard;
            default:
                return CombatEstimateDifficulty.Easy;
        }
    }

    public void ExecuteTransition(IScreen currentScreen, IScreenManager screenManager, HeroCampaign currentCampaign)
    {
        screenManager.ExecuteTransition(currentScreen, ScreenTransition.Combat,
            new CombatScreenTransitionArguments(currentCampaign, CombatSequence, 0, false, _location, null){ IsGoalStage = IsGoalStage});
    }
    
    /// <inheritdoc/>
    public bool IsGoalStage { get; init; }
}