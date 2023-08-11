using System.Collections.Generic;

using Client.Core;
using Client.Core.Campaigns;
using Client.ScreenManagement;

namespace Client.GameScreens.Armory;

internal sealed class ArmoryScreenTransitionArguments : IScreenTransitionArguments
{
    public ArmoryScreenTransitionArguments(HeroCampaign currentCampaign, IReadOnlyList<Equipment> availableEquipment)
    {
        CurrentCampaign = currentCampaign;
        AvailableEquipment = availableEquipment;
    }

    public IReadOnlyList<Equipment> AvailableEquipment { get; }

    public HeroCampaign CurrentCampaign { get; }
}
