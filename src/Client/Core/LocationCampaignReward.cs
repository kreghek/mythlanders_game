using Client.GameScreens.CampaignReward;

namespace Client.Core;

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