using Client.Core.CampaignEffects;
using Client.GameScreens;

using Core.Props;

namespace Client.Core.CampaignRewards;

internal sealed class ResourceCampaignReward : ICampaignEffect
{
    private readonly IProp _resource;

    public ResourceCampaignReward(IProp resource)
    {
        _resource = resource;
    }

    public string GetEffectDisplayText()
    {
        return GameObjectHelper.GetLocalizedProp(_resource.Scheme.Sid);
    }
}