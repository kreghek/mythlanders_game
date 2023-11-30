using Client.Core;

namespace Client.GameScreens.CampaignReward;

internal sealed class LocationCampaignReward : ICampaignReward
{
    public ILocationSid Location { get; }

    public LocationCampaignReward(ILocationSid location)
    {
        Location = location;
    }

    public string GetRewardName()
    {
        return Location.ToString() ?? "[loc]";
    }
}