using Rpg.Client.Assets.Equipments.Herbalist;
using Rpg.Client.Core;

namespace Rpg.Client.Assets.Heroes
{
    internal class HerbalistFactory : HeroFactoryBase
    {
        public override UnitName HeroName => UnitName.Herbalist;

        protected override IEquipmentScheme[] GetEquipment()
        {
            return new IEquipmentScheme[]
            {
                new HerbBag(),
                new WomanShort(),
                new BookOfHerbs()
            };
        }

        protected override IUnitLevelScheme[] GetLevels()
        {
            return new IUnitLevelScheme[]
            {
            };
        }
    }
}