namespace Client.Core.CampaignEffects;

internal sealed class MonsterPerkCampaignEffect : ICampaignEffect
{
    private readonly IPerk _perk;

    public MonsterPerkCampaignEffect(IPerk perk)
    {
        _perk = perk;
    }

    public void Apply(Globe globe)
    {
        globe.Player.AddMonsterPerk(_perk);
    }

    public string GetEffectDisplayText()
    {
        throw new System.NotImplementedException();
    }
}