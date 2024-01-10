using Client.GameScreens;

namespace Client.Core.CampaignRewards;

internal sealed class LocationCampaignReward : ICampaignReward
{
    public LocationCampaignReward(ILocationSid location)
    {
        Location = location;
    }

    public ILocationSid Location { get; }

    public string GetRewardName()
    {
        return GameObjectHelper.GetLocalized(Location);
    }
}