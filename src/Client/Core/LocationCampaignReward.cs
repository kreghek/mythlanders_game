using Client.GameScreens.CampaignReward;

namespace Client.Core;

internal sealed class LocationCampaignReward : ICampaignReward
{
    public LocationCampaignReward(ILocationSid location)
    {
        Location = location;
    }

    public ILocationSid Location { get; }

    public string GetRewardName()
    {
        return Location.ToString() ?? "[loc]";
    }
}