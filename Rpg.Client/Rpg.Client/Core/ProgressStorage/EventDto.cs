namespace Rpg.Client.Core.ProgressStorage
{
    internal sealed record EventDto
    {
        public int Counter { get; set; }
        public string Sid { get; init; }
    }
}