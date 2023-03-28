using System.Collections.Generic;

using Core.Crises;

namespace Client.Assets.Crises;

public interface ICrisesCatalog
{
    public IReadOnlyCollection<ICrisis> GetAll();
}