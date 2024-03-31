using System.Collections.Generic;

namespace Client.Core;

public interface IGameProgressionTransition
{
    IEnumerable<GameProgressionEntry> Entries { get; }
    IGameProgressionTransition Next { get; }
    IEnumerable<GameProgressionTrigger> Triggers { get; }
}