using System.Collections.Generic;

namespace Rpg.Client.Core
{
    internal interface IMonsterGenerationGlobeContext
    {
        IReadOnlyCollection<BiomeCulture> BiomesWithBosses { get; }
        int GlobeProgressLevel { get; }
    }
}