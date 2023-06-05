using System.Collections.Generic;

using Client.Core;

namespace Rpg.Client.Core
{
    internal interface IStoryPointCatalog
    {
        IReadOnlyCollection<IStoryPoint> GetAll();
    }
}