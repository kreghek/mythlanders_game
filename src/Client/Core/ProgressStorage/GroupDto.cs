using System.Collections.Generic;

namespace Client.Core.ProgressStorage;

internal sealed record GroupDto
{
    public IEnumerable<PlayerUnitDto> Units { get; init; }
}