using Rpg.Client.Core;

namespace Rpg.Client.GameScreens
{
    internal static class BackgroundHelper
    {
        public static BackgroundType GetBackgroundType(LocationSid regularTheme)
        {
            return regularTheme switch
            {
                LocationSid.Thicket => BackgroundType.SlavicDarkThicket,
                LocationSid.Battleground => BackgroundType.SlavicBattleground,
                LocationSid.Swamp => BackgroundType.SlavicSwamp,
                LocationSid.DestroyedVillage => BackgroundType.SlavicDestroyedVillage,

                LocationSid.Monastery => BackgroundType.ChineseMonastery,

                LocationSid.Desert => BackgroundType.EgyptianDesert,
                LocationSid.SacredPlace => BackgroundType.EgyptianPyramids,

                LocationSid.ShipGraveyard => BackgroundType.GreekShipGraveyard,

                _ => BackgroundType.SlavicBattleground
            };
        }
    }
}