namespace Client.Core.CampaignRewards;

internal class AddGlobalEffectCampaignReward : ICampaignReward
{
    public AddGlobalEffectCampaignReward(IGlobeEvent targetGlobeEvent)
    {
        TargetGlobeEvent = targetGlobeEvent;
    }

    internal IGlobeEvent TargetGlobeEvent { get; }

    public string GetRewardName()
    {
        return TargetGlobeEvent.Title;
    }
}