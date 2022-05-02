using System.Collections.Generic;

namespace Rpg.Client.Core
{
    internal interface IMonsterGenerationGlobeContext
    {
        int GlobeProgressLevel { get; }

        IReadOnlyCollection<BiomeType> BiomesWithBosses { get; }
    }
}