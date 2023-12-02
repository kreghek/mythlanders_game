using Client.GameScreens;
using Client.GameScreens.CampaignReward;

namespace Client.Core;

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