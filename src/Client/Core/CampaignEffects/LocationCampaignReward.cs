using Client.Core.CampaignEffects;
using Client.GameScreens;

namespace Client.Core.CampaignRewards;

internal sealed class LocationCampaignReward : ICampaignEffect
{
    public LocationCampaignReward(ILocationSid location)
    {
        Location = location;
    }

    public ILocationSid Location { get; }

    public string GetEffectDisplayText()
    {
        return GameObjectHelper.GetLocalized(Location);
    }
}