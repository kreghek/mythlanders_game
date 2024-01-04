using System.Collections.Generic;

using Core.Crises;

namespace Client.Assets.Catalogs.Crises;

public static class CrisesCatalogSids
{
    public static string CityWitchHunting => nameof(CityWitchHunting);
    public static string CrystalTreasures => nameof(CrystalTreasures);
    public static string DroneRemains => nameof(DroneRemains);
    public static string InfernalSickness => nameof(InfernalSickness);
    public static string JinkCity => nameof(JinkCity);
    public static string MagicTrap => nameof(MagicTrap);
    public static string Starvation => nameof(Starvation);
}

public sealed class CrisesCatalog : ICrisesCatalog
{
    private readonly ICrisis[] _crises;

    public CrisesCatalog()
    {
        _crises = new ICrisis[]
        {
            new Crisis(CrisesCatalogSids.MagicTrap),
            new Crisis(CrisesCatalogSids.CityWitchHunting),
            new Crisis(CrisesCatalogSids.JinkCity),
            new Crisis(CrisesCatalogSids.InfernalSickness),
            new Crisis(CrisesCatalogSids.Starvation),
            new Treasures(CrisesCatalogSids.CrystalTreasures),
            new Treasures(CrisesCatalogSids.DroneRemains)

            //new Crisis("Preying", new ICrisisAftermath[]
            //{
            //    new DamageSingleRandomOptionAftermath("Prey", dice),
            //    new DamageAllHeroesOptionAftermath("Ignore")
            //}),
            ////new Crisis("DesertStorm", new ICrisisAftermath[]
            ////{
            ////    new DamageSingleRandomCrisisAftermath("RunAway"),
            ////    new DamageAllCrisisAftermath("Hide")
            ////}),
            ////new Crisis("FireCaster", new ICrisisAftermath[]
            ////{
            ////    new DamageSingleRandomCrisisAftermath("RunAway"),
            ////    new DamageAllCrisisAftermath("Hide")
            ////}),

            ////new Treasues("Treasures", new ICrisisAftermath[]
            ////{
            ////    new DamageSingleRandomCrisisAftermath("TakeAll"),
            ////    new DamageAllCrisisAftermath("Examine")
            ////}),
            //new Treasues("CultOfStar", new ICrisisAftermath[]
            //{
            //    new RestSingleRandomCrisisAftermath("Obey", dice),
            //    new RestAllOptionAftermath("ObeyAll")
            //}),

            //new Treasues("Tavern", new ICrisisAftermath[]
            //{
            //    new RestoreAllButSingleRandomNotOptionAftermath("Rest", dice),
            //    new RestAllOptionAftermath("RestAll")
            //})
        };
    }

    public IReadOnlyCollection<ICrisis> GetAll()
    {
        return _crises;
    }
}