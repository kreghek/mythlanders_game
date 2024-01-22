namespace Client.Core.CampaignEffects;

internal interface ICampaignEffect
{
    void Apply(Globe globe);
    string GetEffectDisplayText();
}