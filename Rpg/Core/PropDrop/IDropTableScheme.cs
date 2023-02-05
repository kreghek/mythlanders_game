using Core.Props;

namespace Core.PropDrop
{
    public interface IDropTableScheme : IScheme
    {
        IDropTableRecordSubScheme[]? Records { get; }
        int Rolls { get; }
    }
}
