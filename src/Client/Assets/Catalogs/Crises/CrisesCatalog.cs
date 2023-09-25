using System.Collections.Generic;

using Core.Crises;

namespace Client.Assets.Catalogs.Crises;

public sealed class CrisesCatalog : ICrisesCatalog
{
    public readonly ICrisis[] _crises;

    public CrisesCatalog()
    {
        _crises = new ICrisis[]
        {
            new Crisis("MagicTrap"),
            new Crisis("CityWitchHunting")
            //new Crisis("InfernalSickness", new ICrisisAftermath[]
            //{
            //    new DamageSingleRandomOptionAftermath("RunOut", dice),
            //    new DamageAllHeroesOptionAftermath("DestroyTheSickSource")
            //}),
            //new Crisis("Starvation", new ICrisisAftermath[]
            //{
            //    new DamageSingleRandomOptionAftermath("StarveSolo", dice),
            //    new DamageAllHeroesOptionAftermath("StarveAll")
            //    //new DamageAllCrisisAftermath("BuyFood")
            //}),
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
            ////new Crisis("Bandits", new ICrisisAftermath[]
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
            ////new Treasues("Drone", new ICrisisAftermath[]
            ////{
            ////    new DamageSingleRandomCrisisAftermath("TakeAll"),
            ////    new DamageAllCrisisAftermath("Examine")
            ////}),
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