namespace Client.Core.CampaignRewards;
internal class AddGlobalEffectCampaignReward : ICampaignReward
{
    private readonly IGlobeEvent _targetGlobeEvent;

    public AddGlobalEffectCampaignReward(IGlobeEvent targetGlobeEvent) 
    {
        _targetGlobeEvent = targetGlobeEvent;
    }

    internal IGlobeEvent TargetGlobeEvent => _targetGlobeEvent;

    public string GetRewardName()
    {
        return TargetGlobeEvent.Title;
    }
}
