namespace Client.Core.CampaignEffects;

internal sealed class UnlockFeatureCampaignEffect : ICampaignEffect
{
    private readonly GameFeature _gameFeature;

    public UnlockFeatureCampaignEffect(GameFeature gameFeature)
    {
        _gameFeature = gameFeature;
    }

    public void Apply(Globe globe)
    {
        globe.Features.AddFeature(_gameFeature);
    }

    public string GetEffectDisplayText()
    {
        return _gameFeature.Value;
    }
}