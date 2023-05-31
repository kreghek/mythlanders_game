using Client.Core;

using Rpg.Client.Assets.Equipments.Swordsman;
using Rpg.Client.Assets.GraphicConfigs.Heroes;
using Rpg.Client.Core;

namespace Rpg.Client.Assets.Heroes
{
    internal class SwordsmanFactory : HeroFactoryBase
    {
        public override UnitName HeroName => UnitName.Swordsman;

        protected override IEquipmentScheme[] GetEquipment()
        {
            return new IEquipmentScheme[]
            {
                new CombatSword(),
                new Mk2MediumPowerArmor(),
                new WoodenHandSculpture()
            };
        }

        protected override UnitGraphicsConfigBase GetGraphicsConfig()
        {
            return new SwordsmanGraphicsConfig();
        }

        protected override IUnitLevelScheme[] GetLevels()
        {
            return new IUnitLevelScheme[]
            {
            };
        }
    }
}