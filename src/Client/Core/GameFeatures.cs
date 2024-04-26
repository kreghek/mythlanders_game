namespace Client.Core;

public static class GameFeatures
{
    public static GameFeature CampaignMap { get; } = new(nameof(CampaignMap));
    public static GameFeature Campaigns { get; } = new(nameof(Campaigns));
    public static GameFeature SideQuests { get; } = new(nameof(SideQuests));
    public static GameFeature SlidingPuzzleMiniGame{ get; } = new(nameof(SlidingPuzzleMiniGame));
    public static GameFeature Match3MiniGame{ get; } = new(nameof(Match3MiniGame));
    public static GameFeature TowersMiniGame{ get; } = new(nameof(TowersMiniGame));
}