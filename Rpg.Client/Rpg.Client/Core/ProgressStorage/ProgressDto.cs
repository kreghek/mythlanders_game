using System.Collections.Generic;

namespace Rpg.Client.Core.ProgressStorage
{
    internal sealed record ProgressDto
    {
        public PlayerDto? Player { get; init; }
        public IEnumerable<EventDto?>? Events { get; init; }
        public IEnumerable<BiomeDto> Biomes { get; init; }
    }
}