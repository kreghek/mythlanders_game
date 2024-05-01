namespace Client.Core;

public static class GameFeatures
{
    public static GameFeature CampaignMap { get; } = new(nameof(CampaignMap));
    public static GameFeature Campaigns { get; } = new(nameof(Campaigns));
    public static GameFeature Match3MiniGame { get; } = new(nameof(Match3MiniGame));
    public static GameFeature RewardMonsterPerks { get; } = new(nameof(RewardMonsterPerks));
    public static GameFeature SideQuests { get; } = new(nameof(SideQuests));
    public static GameFeature SlidingPuzzleMiniGame { get; } = new(nameof(SlidingPuzzleMiniGame));
    public static GameFeature TowersMiniGame { get; } = new(nameof(TowersMiniGame));
    public static GameFeature UseMonsterPerks { get; } = new(nameof(UseMonsterPerks));
    public static GameFeature ExecutableQuests { get; } = new(nameof(ExecutableQuests));
    public static GameFeature CampaignEffects { get; } = new(nameof(CampaignEffects));
}