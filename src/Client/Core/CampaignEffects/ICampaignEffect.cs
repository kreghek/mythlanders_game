namespace Client.Core.CampaignEffects;

internal interface ICampaignEffect
{
    string GetEffectDisplayText();

    void Apply(ICampaignEffectContext context);
}

internal interface ICampaignEffectContext
{

}