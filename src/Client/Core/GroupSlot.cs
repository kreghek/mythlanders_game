using Client.Core.Heroes;

namespace Client.Core;

internal sealed class GroupSlot<T>
{
    public T? Hero { get; set; }
    public int Index { get; init; }
}