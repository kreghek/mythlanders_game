using Client.Core.CampaignEffects;

namespace Client.Core.CampaignRewards;

internal class AddGlobalEffectCampaignReward : ICampaignEffect
{
    public AddGlobalEffectCampaignReward(IGlobeEvent targetGlobeEvent)
    {
        TargetGlobeEvent = targetGlobeEvent;
    }

    internal IGlobeEvent TargetGlobeEvent { get; }

    public string GetEffectDisplayText()
    {
        return TargetGlobeEvent.Title;
    }
}