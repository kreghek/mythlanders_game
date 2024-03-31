using System.Collections.Generic;

namespace Client.Core;

public record GameProgressionTransition(IEnumerable<GameProgressionTrigger> Triggers, IEnumerable<GameProgressionEntry> Entries, IGameProgressionTransition Next) : IGameProgressionTransition;