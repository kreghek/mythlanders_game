using System;

using Client.Core.Heroes;

namespace Client.Core;

internal sealed class AddPerkUnitLevel<TPerk> : UnitLevelBase where TPerk : IPerk, new()
{
    public AddPerkUnitLevel(int level) : base(level)
    {
    }

    public override void Apply(Hero unit)
    {
        var perk = Activator.CreateInstance<TPerk>();
        unit.Perks.Add(perk);
    }
}