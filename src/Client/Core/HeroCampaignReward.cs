using Client.GameScreens;
using Client.GameScreens.CampaignReward;

namespace Client.Core;

internal sealed class HeroCampaignReward : ICampaignReward
{
    private readonly UnitName _hero;

    public HeroCampaignReward(UnitName hero)
    {
        _hero = hero;
    }

    public UnitName Hero => _hero;

    public string GetRewardName()
    {
        return GameObjectHelper.GetLocalized(Hero);
    }
}