using System.Collections.Generic;

namespace Client.Core;

internal interface IStoryPointCatalog
{
    IReadOnlyCollection<IStoryPoint> GetAll();
}