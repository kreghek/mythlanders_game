using Client.GameScreens;

namespace Client.Core.CampaignEffects;

internal sealed class UnlockHeroCampaignEffect : ICampaignEffect
{
    public UnlockHeroCampaignEffect(UnitName hero)
    {
        Hero = hero;
    }

    public UnitName Hero { get; }

    public void Apply(Globe globe)
    {
        globe.Player.AddHero(HeroState.Create(Hero.ToString()));
    }

    public string GetEffectDisplayText()
    {
        return GameObjectHelper.GetLocalized(Hero);
    }
}