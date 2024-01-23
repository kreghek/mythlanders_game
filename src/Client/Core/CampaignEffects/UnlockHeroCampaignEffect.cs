using Client.GameScreens;

namespace Client.Core.CampaignEffects;

internal sealed class UnlockHeroCampaignEffect : ICampaignEffect
{
    public UnlockHeroCampaignEffect(string heroSid)
    {
        Hero = heroSid;
    }

    public string Hero { get; }

    public void Apply(Globe globe)
    {
        globe.Player.AddHero(HeroState.Create(Hero));
    }

    public string GetEffectDisplayText()
    {
        return GameObjectHelper.GetLocalized(Hero);
    }
}