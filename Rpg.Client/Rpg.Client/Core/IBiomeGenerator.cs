using System.Collections.Generic;

namespace Rpg.Client.Core
{
    internal interface IBiomeGenerator
    {
        IReadOnlyList<Biome> Generate();
    }
}