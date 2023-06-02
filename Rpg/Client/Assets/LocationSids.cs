using Client.Core;

namespace Client.Assets;

internal static class LocationSids
{
    [BiomeCulture(BiomeCulture.Slavic)]
    public static ILocationSid Thicket = new LocationSid(nameof(Thicket));
    [BiomeCulture(BiomeCulture.Slavic)]
    public static ILocationSid Swamp = new LocationSid(nameof(Swamp));
    [BiomeCulture(BiomeCulture.Slavic)]
    public static ILocationSid Pit = new LocationSid(nameof(Pit));
    [BiomeCulture(BiomeCulture.Slavic)]
    public static ILocationSid Battleground = new LocationSid(nameof(Battleground));
    [BiomeCulture(BiomeCulture.Slavic)]
    public static ILocationSid DeathPath = new LocationSid(nameof(DeathPath));
    [BiomeCulture(BiomeCulture.Slavic)]
    public static ILocationSid Mines = new LocationSid(nameof(Mines));
    [BiomeCulture(BiomeCulture.Slavic)]
    public static ILocationSid DestroyedVillage = new LocationSid(nameof(DestroyedVillage));
    [BiomeCulture(BiomeCulture.Slavic)]
    public static ILocationSid Castle = new LocationSid(nameof(Castle));

    [BiomeCulture(BiomeCulture.Chinese)]
    public static ILocationSid Monastery = new LocationSid(nameof(Monastery));
    [BiomeCulture(BiomeCulture.Chinese)]
    public static ILocationSid GiantBamboo = new LocationSid(nameof(GiantBamboo));
    [BiomeCulture(BiomeCulture.Chinese)]
    public static ILocationSid EmperorTomb = new LocationSid(nameof(EmperorTomb));
    [BiomeCulture(BiomeCulture.Chinese)]
    public static ILocationSid SecretTown = new LocationSid(nameof(SecretTown));
    [BiomeCulture(BiomeCulture.Chinese)]
    public static ILocationSid GreatWall = new LocationSid(nameof(GreatWall));
    [BiomeCulture(BiomeCulture.Chinese)]
    public static ILocationSid RiseFields = new LocationSid(nameof(RiseFields));
    [BiomeCulture(BiomeCulture.Chinese)]
    public static ILocationSid DragonOolong = new LocationSid(nameof(DragonOolong));
    [BiomeCulture(BiomeCulture.Chinese)]
    public static ILocationSid SkyTower = new LocationSid(nameof(SkyTower));

    [BiomeCulture(BiomeCulture.Egyptian)]
    public static ILocationSid Desert = new LocationSid(nameof(Desert));
    [BiomeCulture(BiomeCulture.Egyptian)]
    public static ILocationSid SacredPlace = new LocationSid(nameof(SacredPlace));
    [BiomeCulture(BiomeCulture.Egyptian)]
    public static ILocationSid Temple = new LocationSid(nameof(Temple));
    [BiomeCulture(BiomeCulture.Egyptian)]
    public static ILocationSid Oasis = new LocationSid(nameof(Oasis));
    [BiomeCulture(BiomeCulture.Egyptian)]
    public static ILocationSid Obelisk = new LocationSid(nameof(Obelisk));
    [BiomeCulture(BiomeCulture.Egyptian)]
    public static ILocationSid ScreamValley = new LocationSid(nameof(ScreamValley));

    [BiomeCulture(BiomeCulture.Greek)]
    public static ILocationSid ShipGraveyard = new LocationSid(nameof(ShipGraveyard));
    [BiomeCulture(BiomeCulture.Greek)]
    public static ILocationSid Vines = new LocationSid(nameof(Vines));
    [BiomeCulture(BiomeCulture.Greek)]
    public static ILocationSid Garden = new LocationSid(nameof(Garden));
    [BiomeCulture(BiomeCulture.Greek)]
    public static ILocationSid Palace = new LocationSid(nameof(Palace));
    [BiomeCulture(BiomeCulture.Greek)]
    public static ILocationSid Labyrinth = new LocationSid(nameof(Labyrinth));

    [BiomeCulture(BiomeCulture.Cosmos)]
    public static ILocationSid ChangingLabyrinth = new LocationSid(nameof(ChangingLabyrinth));
    [BiomeCulture(BiomeCulture.Cosmos)]
    public static ILocationSid Nothing = new LocationSid(nameof(Nothing));
    [BiomeCulture(BiomeCulture.Cosmos)]
    public static ILocationSid Epicenter = new LocationSid(nameof(Epicenter));
}