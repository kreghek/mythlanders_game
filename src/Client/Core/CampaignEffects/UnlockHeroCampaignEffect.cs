using Client.Core.CampaignEffects;
using Client.GameScreens;

namespace Client.Core.CampaignRewards;

internal sealed class UnlockHeroCampaignEffect : ICampaignEffect
{
    public UnlockHeroCampaignEffect(UnitName hero)
    {
        Hero = hero;
    }

    public UnitName Hero { get; }

    public string GetEffectDisplayText()
    {
        return GameObjectHelper.GetLocalized(Hero);
    }
}