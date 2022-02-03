using Rpg.Client.Assets.Equipments;
using Rpg.Client.Assets.Perks;
using Rpg.Client.Assets.Skills;
using Rpg.Client.Core;
using Rpg.Client.Core.GraphicConfigs;

namespace Rpg.Client.Assets.Heroes
{
    internal class ArcherFactory : HeroFactoryBase
    {
        public override UnitName UnitName => UnitName.Hawk;

        protected override UnitGraphicsConfigBase GetGraphicsConfig()
        {
            return new HawkGraphicsConfig();
        }

        protected override IEquipmentScheme[] GetEquipment()
        {
            return new IEquipmentScheme[]
                {
                    new ArcherPulsarBow(),
                    new Mk3ScoutPowerArmor(),
                    new SilverWindNecklace()
                };
        }

        protected override IUnitLevelScheme[] GetLevels()
        {
            return new IUnitLevelScheme[]
                {
                    new AddSkillUnitLevel(1, new EnergyShotSkill()),
                    new AddSkillUnitLevel(2, new RapidShotSkill()),
                    new AddPerkUnitLevel(2, new CriticalHit()),
                    new AddSkillUnitLevel(3, new ArrowRainSkill(true)),
                    new AddSkillUnitLevel(4, new ZduhachMightSkill(true))
                };
        }
    }
}