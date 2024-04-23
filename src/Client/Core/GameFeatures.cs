namespace Client.Core;

public static class GameFeatures
{
    public static GameFeature CampaignMap { get; } = new GameFeature(nameof(CampaignMap));
    public static GameFeature Campaigns { get; } = new GameFeature(nameof(Campaigns));
    public static GameFeature SideQuests { get; } = new GameFeature(nameof(SideQuests));
}