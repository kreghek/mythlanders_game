using Client.Core;

namespace Client.Assets;

internal static class LocationSids
{
    [BiomeCulture(LocationCulture.Slavic)]
    public static ILocationSid Thicket = new LocationSid(nameof(Thicket));
    [BiomeCulture(LocationCulture.Slavic)]
    public static ILocationSid Swamp = new LocationSid(nameof(Swamp));
    [BiomeCulture(LocationCulture.Slavic)]
    public static ILocationSid Pit = new LocationSid(nameof(Pit));
    [BiomeCulture(LocationCulture.Slavic)]
    public static ILocationSid Battleground = new LocationSid(nameof(Battleground));
    [BiomeCulture(LocationCulture.Slavic)]
    public static ILocationSid DeathPath = new LocationSid(nameof(DeathPath));
    [BiomeCulture(LocationCulture.Slavic)]
    public static ILocationSid Mines = new LocationSid(nameof(Mines));
    [BiomeCulture(LocationCulture.Slavic)]
    public static ILocationSid DestroyedVillage = new LocationSid(nameof(DestroyedVillage));
    [BiomeCulture(LocationCulture.Slavic)]
    public static ILocationSid Castle = new LocationSid(nameof(Castle));

    [BiomeCulture(LocationCulture.Chinese)]
    public static ILocationSid Monastery = new LocationSid(nameof(Monastery));
    [BiomeCulture(LocationCulture.Chinese)]
    public static ILocationSid GiantBamboo = new LocationSid(nameof(GiantBamboo));
    [BiomeCulture(LocationCulture.Chinese)]
    public static ILocationSid EmperorTomb = new LocationSid(nameof(EmperorTomb));
    [BiomeCulture(LocationCulture.Chinese)]
    public static ILocationSid SecretTown = new LocationSid(nameof(SecretTown));
    [BiomeCulture(LocationCulture.Chinese)]
    public static ILocationSid GreatWall = new LocationSid(nameof(GreatWall));
    [BiomeCulture(LocationCulture.Chinese)]
    public static ILocationSid RiseFields = new LocationSid(nameof(RiseFields));
    [BiomeCulture(LocationCulture.Chinese)]
    public static ILocationSid DragonOolong = new LocationSid(nameof(DragonOolong));
    [BiomeCulture(LocationCulture.Chinese)]
    public static ILocationSid SkyTower = new LocationSid(nameof(SkyTower));

    [BiomeCulture(LocationCulture.Egyptian)]
    public static ILocationSid Desert = new LocationSid(nameof(Desert));
    [BiomeCulture(LocationCulture.Egyptian)]
    public static ILocationSid SacredPlace = new LocationSid(nameof(SacredPlace));
    [BiomeCulture(LocationCulture.Egyptian)]
    public static ILocationSid Temple = new LocationSid(nameof(Temple));
    [BiomeCulture(LocationCulture.Egyptian)]
    public static ILocationSid Oasis = new LocationSid(nameof(Oasis));
    [BiomeCulture(LocationCulture.Egyptian)]
    public static ILocationSid Obelisk = new LocationSid(nameof(Obelisk));
    [BiomeCulture(LocationCulture.Egyptian)]
    public static ILocationSid ScreamValley = new LocationSid(nameof(ScreamValley));

    [BiomeCulture(LocationCulture.Greek)]
    public static ILocationSid ShipGraveyard = new LocationSid(nameof(ShipGraveyard));
    [BiomeCulture(LocationCulture.Greek)]
    public static ILocationSid Vines = new LocationSid(nameof(Vines));
    [BiomeCulture(LocationCulture.Greek)]
    public static ILocationSid Garden = new LocationSid(nameof(Garden));
    [BiomeCulture(LocationCulture.Greek)]
    public static ILocationSid Palace = new LocationSid(nameof(Palace));
    [BiomeCulture(LocationCulture.Greek)]
    public static ILocationSid Labyrinth = new LocationSid(nameof(Labyrinth));

    [BiomeCulture(LocationCulture.Cosmos)]
    public static ILocationSid ChangingLabyrinth = new LocationSid(nameof(ChangingLabyrinth));
    [BiomeCulture(LocationCulture.Cosmos)]
    public static ILocationSid Nothing = new LocationSid(nameof(Nothing));
    [BiomeCulture(LocationCulture.Cosmos)]
    public static ILocationSid Epicenter = new LocationSid(nameof(Epicenter));
}