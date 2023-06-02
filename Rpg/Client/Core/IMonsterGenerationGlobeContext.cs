using System.Collections.Generic;

using Client.Assets;

namespace Rpg.Client.Core
{
    internal interface IMonsterGenerationGlobeContext
    {
        IReadOnlyCollection<BiomeCulture> BiomesWithBosses { get; }
        int GlobeProgressLevel { get; }
    }
}