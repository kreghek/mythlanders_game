using System.Collections.Generic;

namespace Rpg.Client.Core
{
    internal interface IUnitSchemeCatalog
    {
        IDictionary<UnitName, UnitScheme> PlayerUnits { get; }
        IReadOnlyCollection<UnitScheme> AllMonsters { get; }
    }
}