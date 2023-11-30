using Client.Core;

namespace Client.GameScreens.CampaignReward;

internal sealed class HeroCampaignReward : ICampaignReward
{
    private readonly UnitName _hero;

    public HeroCampaignReward(UnitName hero)
    {
        _hero = hero;
    }

    public string GetRewardDescription()
    {
        return _hero.ToString() ?? "[hero]";
    }
}