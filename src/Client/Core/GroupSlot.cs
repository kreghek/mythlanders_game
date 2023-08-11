using Client.Core.Heroes;

namespace Client.Core;

internal sealed class GroupSlot
{
    public Hero? Hero { get; set; }
    public int Index { get; init; }
}