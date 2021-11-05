using Rpg.Client.Core;

namespace Rpg.Client.Models
{
    internal static class BackgroundHelper
    {
        public static BackgroundType GetBackgroundType(GlobeNodeSid regularTheme)
        {
            return regularTheme switch
            {
                GlobeNodeSid.Thicket => BackgroundType.SlavicDarkThiket,
                GlobeNodeSid.Battleground => BackgroundType.SlavicBattleground,
                GlobeNodeSid.Swamp => BackgroundType.SlavicSwamp,
                _ => BackgroundType.SlavicBattleground
            };
        }
    }
}