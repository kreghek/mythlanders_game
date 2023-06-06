using System.Collections.Generic;

namespace Client.Core;

internal interface IUnitSchemeCatalog
{
    IReadOnlyCollection<UnitScheme> AllMonsters { get; }
    IDictionary<UnitName, UnitScheme> Heroes { get; }
}