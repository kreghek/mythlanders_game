using Client.Core;
using Client.Core.Campaigns;
using Client.GameScreens.Combat;
using Client.ScreenManagement;

namespace Client.Assets.StageItems;

internal sealed class CombatStageItem : ICampaignStageItem
{
    private readonly ILocationSid _location;

    public CombatStageItem(ILocationSid location, CombatSequence combatSequence, CombatMetadata metadata)
    {
        _location = location;
        CombatSequence = combatSequence;

        Metadata = metadata;
    }

    public CombatMetadata Metadata { get; }

    internal CombatSequence CombatSequence { get; }

    public void ExecuteTransition(IScreen currentScreen, IScreenManager screenManager, HeroCampaign currentCampaign)
    {
        screenManager.ExecuteTransition(currentScreen, ScreenTransition.Combat,
            new CombatScreenTransitionArguments(currentCampaign, CombatSequence, 0, false, _location, null)
                { IsGoalStage = IsGoalStage });
    }

    /// <inheritdoc />
    public bool IsGoalStage { get; init; }
}