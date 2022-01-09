namespace Rpg.Client.Core.ProgressStorage
{
    internal sealed record PlayerDto
    {
        public GroupDto Group { get; init; }

        public GroupDto Pool { get; init; }

        public ResourceDto[] Resources { get; init; }
        public string[] KnownMonsterSids { get; init; }
    }
}