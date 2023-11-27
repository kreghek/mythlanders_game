using System.Collections.Generic;

using Client.Core;

namespace Client.Assets.Catalogs.CampaignGeneration;

public static class GameLocations
{
    public static IReadOnlyCollection<ILocationSid> GetGameLocations()
    {
        var allGameLocations = new[]
        {
            LocationSids.Thicket,
            LocationSids.Monastery,
            LocationSids.ShipGraveyard,
            LocationSids.Desert,

            LocationSids.Swamp,

            LocationSids.Battleground
        };

        return allGameLocations;
    }
}