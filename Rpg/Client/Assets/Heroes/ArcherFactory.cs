using Rpg.Client.Assets.Equipments.Archer;
using Rpg.Client.Assets.GraphicConfigs.Heroes;
using Rpg.Client.Core;

namespace Rpg.Client.Assets.Heroes
{
    internal class RobberFactory : HeroFactoryBase
    {
        public override UnitName HeroName => UnitName.Archer;

        protected override IEquipmentScheme[] GetEquipment()
        {
            return new IEquipmentScheme[]
            {
                new ArcherPulsarBow(),
                new Mk3ScoutPowerArmor(),
                new SilverWindNecklace()
            };
        }

        protected override UnitGraphicsConfigBase GetGraphicsConfig()
        {
            return new ArcherGraphicsConfig();
        }

        protected override IUnitLevelScheme[] GetLevels()
        {
            return new IUnitLevelScheme[]
            {
            };
        }
    }
}