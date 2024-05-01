namespace Client.Core;

public static class GameFeatures
{
    public static GameFeature CampaignMap { get; } = new GameFeature(nameof(CampaignMap));
    public static GameFeature Campaigns { get; } = new GameFeature(nameof(Campaigns));
    public static GameFeature SideQuests { get; } = new GameFeature(nameof(SideQuests));
    public static GameFeature UseMonsterPerks { get; } = new GameFeature(nameof(UseMonsterPerks));
    public static GameFeature RewardMonsterPerks { get; } = new GameFeature(nameof(RewardMonsterPerks));
}