using Client.Core.Heroes;

namespace Client.Core;

internal sealed class AddPredefinedPerkUnitLevel : UnitLevelBase
{
    private readonly IPerk _perk;

    public AddPredefinedPerkUnitLevel(int level, IPerk perk) : base(level)
    {
        _perk = perk;
    }

    public override void Apply(Hero unit)
    {
        unit.Perks.Add(_perk);
    }
}