using Client.Assets.CombatMovements;
using System.IO;

using Client.Assets.GraphicConfigs;
using Client.Core;

using Rpg.Client.Assets.Equipments.Sergeant;
using Rpg.Client.Core;

namespace Rpg.Client.Assets.Heroes
{
    internal class PartisanFactory : HeroFactoryBase
    {
        public override UnitName HeroName => UnitName.Partisan;

        protected override IEquipmentScheme[] GetEquipment()
        {
            return new IEquipmentScheme[]
            {
                new CompactSaber(),
                new RedMediumPowerArmor(),
                new MultifunctionalClocks()
            };
        }

        protected override UnitGraphicsConfigBase GetGraphicsConfig()
        {
            return new SingleSpriteGraphicsConfig(Path.Combine(CommonConstants.PathToCharacterSprites, "Heroes", HeroName.ToString(), "Thumbnail"));
        }

        protected override IUnitLevelScheme[] GetLevels()
        {
            return new IUnitLevelScheme[]
            {
            };
        }
    }
}