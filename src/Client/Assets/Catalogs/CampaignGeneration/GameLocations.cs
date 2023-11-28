using System.Collections.Generic;

using Client.Core;

namespace Client.Assets.Catalogs.CampaignGeneration;

/// <summary>
/// Current game location available in the game version.
/// </summary>
public static class GameLocations
{
    /// <summary>
    /// All game locations.
    /// </summary>
    public static IEnumerable<ILocationSid> GetGameLocations()
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