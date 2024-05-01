namespace Client.Core;

public static class GameFeatures
{
    public static GameFeature CampaignMap { get; } = new(nameof(CampaignMap));
    public static GameFeature Campaigns { get; } = new(nameof(Campaigns));
    public static GameFeature RewardMonsterPerks { get; } = new(nameof(RewardMonsterPerks));
    public static GameFeature SideQuests { get; } = new(nameof(SideQuests));
    public static GameFeature UseMonsterPerks { get; } = new(nameof(UseMonsterPerks));
}