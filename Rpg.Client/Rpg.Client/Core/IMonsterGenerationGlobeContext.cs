using System.Collections.Generic;

namespace Rpg.Client.Core
{
    internal interface IMonsterGenerationGlobeContext
    {
        IReadOnlyCollection<BiomeType> BiomesWithBosses { get; }
        int GlobeProgressLevel { get; }
    }
}