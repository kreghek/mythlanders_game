using Rpg.Client.Assets.Equipments.Assaulter;
using Rpg.Client.Assets.GraphicConfigs;
using Rpg.Client.Assets.Perks;
using Rpg.Client.Assets.Skills.Hero.Assaulter;
using Rpg.Client.Core;

namespace Rpg.Client.Assets.Heroes
{
    internal class AssaulterFactory : HeroFactoryBase
    {
        public override UnitName HeroName => UnitName.Assaulter;

        protected override IEquipmentScheme[] GetEquipment()
        {
            return new IEquipmentScheme[]
            {
                new AssaultRifle(),
                new Mk4HeavyPowerArmor(),
                new LuckyPlayCard()
            };
        }

        protected override UnitGraphicsConfigBase GetGraphicsConfig()
        {
            return new SingleSpriteGraphicsConfig();
        }

        protected override IUnitLevelScheme[] GetLevels()
        {
            return new IUnitLevelScheme[]
            {
            };
        }
    }
}