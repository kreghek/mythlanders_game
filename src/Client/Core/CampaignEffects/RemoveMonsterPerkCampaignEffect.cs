namespace Client.Core.CampaignEffects;

internal sealed class RemoveMonsterPerkCampaignEffect : ICampaignEffect
{
    private readonly MonsterPerk _perk;

    public RemoveMonsterPerkCampaignEffect(MonsterPerk perk)
    {
        _perk = perk;
    }

    public void Apply(Globe globe)
    {
        globe.Player.RemoveMonsterPerk(_perk);
    }

    public string GetEffectDisplayText()
    {
        return _perk.Sid;
    }
}