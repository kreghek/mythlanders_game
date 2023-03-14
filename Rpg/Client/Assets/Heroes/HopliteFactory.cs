using Rpg.Client.Assets.Equipments.Legionnaire;
using Rpg.Client.Assets.GraphicConfigs.Heroes;
using Rpg.Client.Assets.Perks;
using Rpg.Client.Assets.Skills.Hero.Hoplite;
using Rpg.Client.Core;

namespace Rpg.Client.Assets.Heroes
{
    internal class HopliteFactory : HeroFactoryBase
    {
        public override UnitName HeroName => UnitName.Hoplite;

        protected override IEquipmentScheme[] GetEquipment()
        {
            return new IEquipmentScheme[]
            {
                new EmberDori(),
                new EmpoweredMk2MediumPowerArmor(),
                new BrokenAresSculpture()
            };
        }

        protected override UnitGraphicsConfigBase GetGraphicsConfig()
        {
            return new HopliteGraphicsConfig();
        }

        protected override IUnitLevelScheme[] GetLevels()
        {
            return new IUnitLevelScheme[]
            {
            };
        }
    }
}