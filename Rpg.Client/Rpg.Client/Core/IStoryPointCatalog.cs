using System.Collections.Generic;

namespace Rpg.Client.Core
{
    internal interface IStoryPointCatalog
    {
        IReadOnlyCollection<IStoryPoint> GetAll();
    }

    interface IStoryPointInitializer
    {
        IReadOnlyCollection<IStoryPoint> Init(Globe globe);
    }
}