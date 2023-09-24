using System.Collections.Generic;

using Core.Crises;

namespace Client.Assets.Catalogs.Crises;

public interface ICrisesCatalog
{
    public IReadOnlyCollection<ICrisis> GetAll();
}