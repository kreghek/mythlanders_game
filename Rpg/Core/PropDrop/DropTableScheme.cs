namespace Core.PropDrop;

public sealed record DropTableScheme(string Sid, IDropTableRecordSubScheme[]? Records, int Rolls) : IDropTableScheme
{
    
}