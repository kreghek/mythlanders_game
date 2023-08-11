using Client.Core.Heroes;

namespace Client.Core;

internal sealed class GroupSlot
{
    public int Index { get; init; }
    public Hero? Hero { get; set; }
}