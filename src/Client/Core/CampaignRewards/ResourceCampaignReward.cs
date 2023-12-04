using Client.GameScreens;

using Core.Props;

namespace Client.Core.CampaignRewards;

internal sealed class ResourceCampaignReward : ICampaignReward
{
    private readonly IProp _resource;

    public ResourceCampaignReward(IProp resource)
    {
        _resource = resource;
    }

    public string GetRewardName()
    {
        return GameObjectHelper.GetLocalizedProp(_resource.Scheme.Sid);
    }
}