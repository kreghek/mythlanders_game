namespace Rpg.Client.Core.ProgressStorage
{
    internal sealed record EventDto
    {
        public string Sid { get; init; }
        public int Counter { get; set; }
    }
}