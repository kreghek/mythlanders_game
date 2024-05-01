namespace Client.Core.CampaignEffects;

internal sealed class AddGlobalEffectCampaignEffect : ICampaignEffect
{
    public AddGlobalEffectCampaignEffect(IGlobeEvent targetGlobeEvent)
    {
        TargetGlobeEvent = targetGlobeEvent;
    }

    internal IGlobeEvent TargetGlobeEvent { get; }

    public void Apply(Globe globe)
    {
        globe.AddGlobalEvent(TargetGlobeEvent);
    }

    public string GetEffectDisplayText()
    {
        return TargetGlobeEvent.TitleSid;
    }
}
