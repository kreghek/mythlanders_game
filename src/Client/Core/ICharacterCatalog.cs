using System.Collections.Generic;

namespace Client.Core;

internal interface ICharacterCatalog
{
    IReadOnlyCollection<UnitScheme> AllMonsters { get; }
    IReadOnlyCollection<string> AvailableHeroes { get; }
}