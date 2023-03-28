using Core.Props;

namespace Core.PropDrop;

public interface IDropTableScheme : IScheme
{
    IDropTableRecordSubScheme[]? Records { get; }
    int Rolls { get; }
}

public sealed record DropTableScheme(IDropTableRecordSubScheme[]? Records, int Rolls) : IDropTableScheme
{
    public string? Sid { get; set; }
}