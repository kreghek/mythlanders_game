using Client.GameScreens;

namespace Client.Core.CampaignEffects;

internal sealed class LocationCampaignEffect : ICampaignEffect
{
    public LocationCampaignEffect(ILocationSid location)
    {
        Location = location;
    }

    public ILocationSid Location { get; }

    public void Apply(Globe globe)
    {
        globe.Player.AddLocation(Location);
    }

    public string GetEffectDisplayText()
    {
        return GameObjectHelper.GetLocalized(Location);
    }
}