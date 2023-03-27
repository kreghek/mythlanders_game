using System.Collections.Generic;

using Core.Crises;

namespace Client.Assets.Crises;

public sealed class CrisesCatalog: ICrisesCatalog
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
            })
        };
    }
    public IReadOnlyCollection<ICrisis> GetAll()
    {
        return _crises;
    }
}