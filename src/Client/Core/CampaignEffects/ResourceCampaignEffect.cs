using Client.GameScreens;

using Core.Props;

namespace Client.Core.CampaignEffects;

internal sealed class ResourceCampaignEffect : ICampaignEffect
{
    private readonly IProp _resource;

    public ResourceCampaignEffect(IProp resource)
    {
        _resource = resource;
    }

    public void Apply(Globe globe)
    {
        globe.Player.Inventory.Add(_resource);
    }

    public string GetEffectDisplayText()
    {
        return GameObjectHelper.GetLocalizedProp(_resource.Scheme.Sid);
    }
}