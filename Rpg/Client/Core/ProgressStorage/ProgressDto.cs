using System.Collections.Generic;

namespace Rpg.Client.Core.ProgressStorage
{
    internal sealed record ProgressDto
    {
        public PlayerDto? Player { get; init; }
    }
}