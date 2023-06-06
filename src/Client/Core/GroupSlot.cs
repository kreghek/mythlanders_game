using Client.Core.Heroes;

namespace Client.Core;

internal sealed class GroupSlot
{
    public int Index { get; init; }
    public bool IsTankLine { get; init; }
    public Hero? Unit { get; set; }
}