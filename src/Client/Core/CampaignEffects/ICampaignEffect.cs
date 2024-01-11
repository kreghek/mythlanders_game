namespace Client.Core.CampaignEffects;

internal interface ICampaignEffect
{
    string GetEffectDisplayText();

    void Apply(Globe globe);
}