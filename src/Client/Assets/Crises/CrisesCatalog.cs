using System.Collections.Generic;

using CombatDicesTeam.Dices;

using Core.Crises;

namespace Client.Assets.Crises;

public sealed class CrisesCatalog : ICrisesCatalog
{
    public readonly ICrisis[] _crises;

    public CrisesCatalog()
    {

        var dice = new LinearDice();

        _crises = new ICrisis[]
        {
            new Crisis("MagicTrap", new ICrisisAftermath[]
            {
                new DamageSingleRandomCrisisAftermath("TakeAllDamageByOneHero", dice),
                new DamageAllCrisisAftermath("DamageTogether")
            }),
            //new Crisis("CityHunting", new ICrisisAftermath[]
            //{
            //    new DamageSingleRandomCrisisAftermath("TryToRunOut"),
            //    new DamageAllCrisisAftermath("TryToFight"),
            //    new DamageAllCrisisAftermath("Conversation")
            //}),
            new Crisis("InfernalSickness", new ICrisisAftermath[]
            {
                new DamageSingleRandomCrisisAftermath("RunOut", dice),
                new DamageAllCrisisAftermath("DestroyTheSickSource")
            }),
            new Crisis("Starvation", new ICrisisAftermath[]
            {
                new DamageSingleRandomCrisisAftermath("Starve", dice),
                new DamageAllCrisisAftermath("StarveAll"),
                new DamageAllCrisisAftermath("BuyFood")
            }),
            new Crisis("Preying", new ICrisisAftermath[]
            {
                new DamageSingleRandomCrisisAftermath("Prey", dice),
                new DamageAllCrisisAftermath("Ignore")
            }),
            //new Crisis("DesertStorm", new ICrisisAftermath[]
            //{
            //    new DamageSingleRandomCrisisAftermath("RunAway"),
            //    new DamageAllCrisisAftermath("Hide")
            //}),
            //new Crisis("Bandits", new ICrisisAftermath[]
            //{
            //    new DamageSingleRandomCrisisAftermath("RunAway"),
            //    new DamageAllCrisisAftermath("Hide")
            //}),
            //new Crisis("FireCaster", new ICrisisAftermath[]
            //{
            //    new DamageSingleRandomCrisisAftermath("RunAway"),
            //    new DamageAllCrisisAftermath("Hide")
            //}),

            //new Treasues("Treasures", new ICrisisAftermath[]
            //{
            //    new DamageSingleRandomCrisisAftermath("TakeAll"),
            //    new DamageAllCrisisAftermath("Examine")
            //}),
            new Treasues("CultOfStar", new ICrisisAftermath[]
            {
                new RestSingleRandomCrisisAftermath("Obey", dice),
                new RestAllCrisisAftermath("ObeyAll")
            }),
            //new Treasues("Drone", new ICrisisAftermath[]
            //{
            //    new DamageSingleRandomCrisisAftermath("TakeAll"),
            //    new DamageAllCrisisAftermath("Examine")
            //}),
            new Treasues("Tavern", new ICrisisAftermath[]
            {
                new RestAllButSingleRandomNotCrisisAftermath("Rest", dice),
                new RestAllCrisisAftermath("RestAll")
            })
        };
    }

    public IReadOnlyCollection<ICrisis> GetAll()
    {
        return _crises;
    }
}