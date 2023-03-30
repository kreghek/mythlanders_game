namespace Core.PropDrop;

public sealed record DropTableScheme(IDropTableRecordSubScheme[]? Records, int Rolls) : IDropTableScheme
{
    public string? Sid { get; set; }
}