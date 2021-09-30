using System.Collections.Generic;

namespace Rpg.Client.Core.ProgressStorage
{
    internal sealed record GroupDto
    {
        public IEnumerable<PlayerUnitDto> Units { get; init; }
    }
}