using Client.GameScreens.CampaignReward;

namespace Client.Core;

internal sealed class HeroCampaignReward : ICampaignReward
{
    private readonly UnitName _hero;

    public HeroCampaignReward(UnitName hero)
    {
        _hero = hero;
    }

    public string GetRewardName()
    {
        return _hero.ToString();
    }
}