using System.Collections.Generic;

namespace Client.Core;

public interface IGameProgressionTransition
{
    IEnumerable<GameProgressionTrigger> Triggers { get; }
    IEnumerable<GameProgressionEntry> Entries { get; }
    IGameProgressionTransition Next { get;}
}
