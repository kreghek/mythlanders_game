using System.Collections.Generic;

namespace Rpg.Client.Core
{
    internal interface IStoryPointInitializer
    {
        IReadOnlyCollection<IStoryPoint> Init(Globe globe);
    }
}