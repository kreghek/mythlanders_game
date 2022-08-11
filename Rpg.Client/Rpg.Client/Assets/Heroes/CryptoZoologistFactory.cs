using Rpg.Client.Assets.Equipments.Zoologist;
using Rpg.Client.Assets.GraphicConfigs;
using Rpg.Client.Assets.Perks;
using Rpg.Client.Assets.Skills.Hero.Assaulter;
using Rpg.Client.Core;

namespace Rpg.Client.Assets.Heroes
{
    internal class CryptoZoologistFactory : HeroFactoryBase
    {
        public override UnitName HeroName => UnitName.Zoologist;

        protected override IEquipmentScheme[] GetEquipment()
        {
            return new IEquipmentScheme[]
            {
                new BioExtractor(),
                new ScientistRobe(),
                new DetectiveOculars()
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