using Rpg.Client.Core;

namespace Rpg.Client.GameScreens
{
    internal static class BackgroundHelper
    {
        public static BackgroundType GetBackgroundType(GlobeNodeSid regularTheme)
        {
            return regularTheme switch
            {
                GlobeNodeSid.Thicket => BackgroundType.SlavicDarkThicket,
                GlobeNodeSid.Battleground => BackgroundType.SlavicBattleground,
                GlobeNodeSid.Swamp => BackgroundType.SlavicSwamp,
                GlobeNodeSid.DestroyedVillage => BackgroundType.SlavicDestroyedVillage,

                GlobeNodeSid.Monastery => BackgroundType.ChineseMonastery,

                GlobeNodeSid.Desert => BackgroundType.EgyptianDisert,
                GlobeNodeSid.SacredPlace => BackgroundType.EgyptianPyramids,

                _ => BackgroundType.SlavicBattleground
            };
        }
    }
}