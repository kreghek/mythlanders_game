using Client.Core;

namespace Client.GameScreens.CampaignReward;

internal interface ICampaignReward
{
    string GetRewardDescription();
}

internal sealed class LocationCampaignReward : ICampaignReward
{
    private readonly ILocationSid _location;

    public LocationCampaignReward(ILocationSid location)
    {
        _location = location;
    }

    public string GetRewardDescription()
    {
        return _location.ToString() ?? "[loc]";
    }
}