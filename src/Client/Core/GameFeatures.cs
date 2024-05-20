﻿namespace Client.Core;

public static class GameFeatures
{
    public static GameFeature CampaignEffects { get; } = new(nameof(CampaignEffects));
    public static GameFeature CampaignMap { get; } = new(nameof(CampaignMap));
    public static GameFeature Campaigns { get; } = new(nameof(Campaigns));
    public static GameFeature Craft { get; } = new(nameof(Craft));
    public static GameFeature ExecutableQuests { get; } = new(nameof(ExecutableQuests));
    public static GameFeature Match3MiniGame { get; } = new(nameof(Match3MiniGame));
    public static GameFeature RewardMonsterPerksCampaignEffect { get; } = new(nameof(RewardMonsterPerksCampaignEffect));
    public static GameFeature RewardResourceCampaignEffect { get; } = new(nameof(RewardResourceCampaignEffect));
    public static GameFeature SacredPlace { get; } = new(nameof(SacredPlace));
    public static GameFeature Shop { get; } = new(nameof(Shop));
    public static GameFeature SideQuests { get; } = new(nameof(SideQuests));
    public static GameFeature SlidingPuzzleMiniGame { get; } = new(nameof(SlidingPuzzleMiniGame));
    public static GameFeature SmallEvents { get; } = new(nameof(SmallEvents));
    public static GameFeature TowersMiniGame { get; } = new(nameof(TowersMiniGame));
    public static GameFeature UseMonsterPerks { get; } = new(nameof(UseMonsterPerks));
}