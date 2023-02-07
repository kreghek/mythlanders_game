using System;

using Client.Core;

namespace Rpg.Client.Core
{
    internal sealed class AddPerkUnitLevel<TPerk> : UnitLevelBase where TPerk : IPerk, new()
    {
        public AddPerkUnitLevel(int level) : base(level)
        {
        }

        public override void Apply(Unit unit)
        {
            var perk = Activator.CreateInstance<TPerk>();
            unit.Perks.Add(perk);
        }
    }
}