namespace Client.Core.CampaignEffects;

internal sealed class MonsterPerkCampaignEffect : ICampaignEffect
{
    private readonly MonsterPerk _perk;

    public MonsterPerkCampaignEffect(MonsterPerk perk)
    {
        _perk = perk;
    }

    public void Apply(Globe globe)
    {
        globe.Player.AddMonsterPerk(_perk);
    }

    public string GetEffectDisplayText()
    {
        return _perk.Sid;
    }
}
