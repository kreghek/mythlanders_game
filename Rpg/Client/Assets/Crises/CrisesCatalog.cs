using System.Collections.Generic;

using Core.Crises;

namespace Client.Assets.Crises;

public sealed class CrisesCatalog : ICrisesCatalog
{
    public readonly ICrisis[] _crises;

    public CrisesCatalog()
    {
        _crises = new[]
        {
            new Crisis("MagicTrap", new ICrisisAftermath[]
            {
                new DamageSingleRandomCrisisAftermath("TakeAllDamageByOneHero"),
                new DamageAllCrisisAftermath("DamageTogether")
            }),
            new Crisis("CityHunting", new ICrisisAftermath[]
            {
                new DamageSingleRandomCrisisAftermath("TryToRunOut"),
                new DamageAllCrisisAftermath("TryToFight"),
                new DamageAllCrisisAftermath("Conversation")
            }),
            new Crisis("InfernalSickness", new ICrisisAftermath[]
            {
                new DamageSingleRandomCrisisAftermath("RunOut"),
                new DamageAllCrisisAftermath("DestroyTheSickSource")
            }),
            new Crisis("Starvation", new ICrisisAftermath[]
            {
                new DamageSingleRandomCrisisAftermath("Starve"),
                new DamageAllCrisisAftermath("StarveAll"),
                new DamageAllCrisisAftermath("BuyFood")
            }),
            new Crisis("Preying", new ICrisisAftermath[]
            {
                new DamageSingleRandomCrisisAftermath("Prey"),
                new DamageAllCrisisAftermath("Ignore")
            })
        };
    }

    public IReadOnlyCollection<ICrisis> GetAll()
    {
        return _crises;
    }
}