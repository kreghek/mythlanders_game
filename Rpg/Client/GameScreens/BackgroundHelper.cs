using Client.Assets;
using Client.Core;

namespace Rpg.Client.GameScreens
{
    internal static class BackgroundHelper
    {
        public static BackgroundType GetBackgroundType(ILocationSid regularTheme)
        {
            return regularTheme.ToString() switch
            {
                nameof(LocationSids.Thicket) => BackgroundType.SlavicDarkThicket,
                nameof(LocationSids.Battleground) => BackgroundType.SlavicBattleground,
                nameof(LocationSids.Swamp) => BackgroundType.SlavicSwamp,
                nameof(LocationSids.DestroyedVillage) => BackgroundType.SlavicDestroyedVillage,

                nameof(LocationSids.Monastery) => BackgroundType.ChineseMonastery,

                nameof(LocationSids.Desert) => BackgroundType.EgyptianDesert,
                nameof(LocationSids.SacredPlace) => BackgroundType.EgyptianPyramids,

                nameof(LocationSids.ShipGraveyard) => BackgroundType.GreekShipGraveyard,

                _ => BackgroundType.SlavicBattleground
            };
        }
    }
}