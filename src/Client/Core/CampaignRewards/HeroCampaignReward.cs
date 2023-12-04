using Client.GameScreens;

namespace Client.Core.CampaignRewards;

internal sealed class HeroCampaignReward : ICampaignReward
{
    public HeroCampaignReward(UnitName hero)
    {
        Hero = hero;
    }

    public UnitName Hero { get; }

    public string GetRewardName()
    {
        return GameObjectHelper.GetLocalized(Hero);
    }
}