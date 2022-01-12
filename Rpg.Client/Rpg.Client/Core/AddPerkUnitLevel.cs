namespace Rpg.Client.Core
{
    internal sealed class AddPerkUnitLevel : UnitLevelBase
    {
        private readonly IPerk _perk;

        public AddPerkUnitLevel(int level, IPerk perk) : base(level)
        {
            _perk = perk;
        }

        public override void Apply(Unit unit)
        {
            unit.Perks.Add(_perk);
        }
    }
}