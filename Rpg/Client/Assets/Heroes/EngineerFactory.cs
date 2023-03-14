using Rpg.Client.Assets.Equipments.Engineer;
using Rpg.Client.Assets.Perks;
using Rpg.Client.Assets.Skills.Hero.Engineer;
using Rpg.Client.Core;

namespace Rpg.Client.Assets.Heroes
{
    internal class EngineerFactory : HeroFactoryBase
    {
        public override UnitName HeroName => UnitName.Engineer;

        protected override IEquipmentScheme[] GetEquipment()
        {
            return new IEquipmentScheme[]
            {
                new FlameThrower(),
                new HeavyCooperHandmadeArmor(),
                new ScientificTableOfMaterials()
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