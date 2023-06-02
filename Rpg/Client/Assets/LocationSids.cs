using Client.Core;

namespace Client.Assets;

internal static class LocationSids
{
    [LocationCulture(LocationCulture.Slavic)]
    public static ILocationSid Thicket { get; } = new LocationSid(nameof(Thicket));
    [LocationCulture(LocationCulture.Slavic)]
    public static ILocationSid Swamp { get; } = new LocationSid(nameof(Swamp));
    [LocationCulture(LocationCulture.Slavic)]
    public static ILocationSid Pit { get; } = new LocationSid(nameof(Pit));
    [LocationCulture(LocationCulture.Slavic)]
    public static ILocationSid Battleground { get; } = new LocationSid(nameof(Battleground));
    [LocationCulture(LocationCulture.Slavic)]
    public static ILocationSid DeathPath { get; } = new LocationSid(nameof(DeathPath));
    [LocationCulture(LocationCulture.Slavic)]
    public static ILocationSid Mines { get; } = new LocationSid(nameof(Mines));
    [LocationCulture(LocationCulture.Slavic)]
    public static ILocationSid DestroyedVillage { get; } = new LocationSid(nameof(DestroyedVillage));
    [LocationCulture(LocationCulture.Slavic)]
    public static ILocationSid Castle { get; } = new LocationSid(nameof(Castle));

    [LocationCulture(LocationCulture.Chinese)]
    public static ILocationSid Monastery { get; } = new LocationSid(nameof(Monastery));
    [LocationCulture(LocationCulture.Chinese)]
    public static ILocationSid GiantBamboo { get; } = new LocationSid(nameof(GiantBamboo));
    [LocationCulture(LocationCulture.Chinese)]
    public static ILocationSid EmperorTomb { get; } = new LocationSid(nameof(EmperorTomb));
    [LocationCulture(LocationCulture.Chinese)]
    public static ILocationSid SecretTown { get; } = new LocationSid(nameof(SecretTown));
    [LocationCulture(LocationCulture.Chinese)]
    public static ILocationSid GreatWall { get; } = new LocationSid(nameof(GreatWall));
    [LocationCulture(LocationCulture.Chinese)]
    public static ILocationSid RiseFields { get; } = new LocationSid(nameof(RiseFields));
    [LocationCulture(LocationCulture.Chinese)]
    public static ILocationSid DragonOolong { get; } = new LocationSid(nameof(DragonOolong));
    [LocationCulture(LocationCulture.Chinese)]
    public static ILocationSid SkyTower { get; } = new LocationSid(nameof(SkyTower));

    [LocationCulture(LocationCulture.Egyptian)]
    public static ILocationSid Desert { get; } = new LocationSid(nameof(Desert));
    [LocationCulture(LocationCulture.Egyptian)]
    public static ILocationSid SacredPlace { get; } = new LocationSid(nameof(SacredPlace));
    [LocationCulture(LocationCulture.Egyptian)]
    public static ILocationSid Temple { get; } = new LocationSid(nameof(Temple));
    [LocationCulture(LocationCulture.Egyptian)]
    public static ILocationSid Oasis { get; } = new LocationSid(nameof(Oasis));
    [LocationCulture(LocationCulture.Egyptian)]
    public static ILocationSid Obelisk { get; } = new LocationSid(nameof(Obelisk));
    [LocationCulture(LocationCulture.Egyptian)]
    public static ILocationSid ScreamValley { get; } = new LocationSid(nameof(ScreamValley));

    [LocationCulture(LocationCulture.Greek)]
    public static ILocationSid ShipGraveyard { get; } = new LocationSid(nameof(ShipGraveyard));
    [LocationCulture(LocationCulture.Greek)]
    public static ILocationSid Vines { get; } = new LocationSid(nameof(Vines));
    [LocationCulture(LocationCulture.Greek)]
    public static ILocationSid Garden { get; } = new LocationSid(nameof(Garden));
    [LocationCulture(LocationCulture.Greek)]
    public static ILocationSid Palace { get; } = new LocationSid(nameof(Palace));
    [LocationCulture(LocationCulture.Greek)]
    public static ILocationSid Labyrinth { get; } = new LocationSid(nameof(Labyrinth));

    [LocationCulture(LocationCulture.Cosmos)]
    public static ILocationSid ChangingLabyrinth { get; } = new LocationSid(nameof(ChangingLabyrinth));
    [LocationCulture(LocationCulture.Cosmos)]
    public static ILocationSid Nothing { get; } = new LocationSid(nameof(Nothing));
    [LocationCulture(LocationCulture.Cosmos)]
    public static ILocationSid Epicenter { get; } = new LocationSid(nameof(Epicenter));
}