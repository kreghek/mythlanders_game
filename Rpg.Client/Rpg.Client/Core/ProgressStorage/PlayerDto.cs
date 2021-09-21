namespace Rpg.Client.Core.ProgressStorage
{
    internal sealed record PlayerDto
    {
        public GroupDto Group { get; init; }

        public GroupDto Pool { get; init; }
    }
}