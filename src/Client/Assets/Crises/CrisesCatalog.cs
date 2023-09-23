using System.Collections.Generic;

using Client.Assets.DialogueOptionAftermath;

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
                new DamageSingleRandomOptionAftermath("TakeAllDamageByOneHero", dice),
                new DamageAllHeroesOptionAftermath("DamageTogether")
            }),
            //new Crisis("CityHunting", new ICrisisAftermath[]
            //{
            //    new DamageSingleRandomCrisisAftermath("TryToRunOut"),
            //    new DamageAllCrisisAftermath("TryToFight"),
            //    new DamageAllCrisisAftermath("Conversation")
            //}),
            new Crisis("InfernalSickness", new ICrisisAftermath[]
            {
                new DamageSingleRandomOptionAftermath("RunOut", dice),
                new DamageAllHeroesOptionAftermath("DestroyTheSickSource")
            }),
            new Crisis("Starvation", new ICrisisAftermath[]
            {
                new DamageSingleRandomOptionAftermath("StarveSolo", dice),
                new DamageAllHeroesOptionAftermath("StarveAll")
                //new DamageAllCrisisAftermath("BuyFood")
            }),
            new Crisis("Preying", new ICrisisAftermath[]
            {
                new DamageSingleRandomOptionAftermath("Prey", dice),
                new DamageAllHeroesOptionAftermath("Ignore")
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
                new RestAllOptionAftermath("ObeyAll")
            }),
            //new Treasues("Drone", new ICrisisAftermath[]
            //{
            //    new DamageSingleRandomCrisisAftermath("TakeAll"),
            //    new DamageAllCrisisAftermath("Examine")
            //}),
            new Treasues("Tavern", new ICrisisAftermath[]
            {
                new RestoreAllButSingleRandomNotOptionAftermath("Rest", dice),
                new RestAllOptionAftermath("RestAll")
            })
        };
    }

    public IReadOnlyCollection<ICrisis> GetAll()
    {
        return _crises;
    }
}