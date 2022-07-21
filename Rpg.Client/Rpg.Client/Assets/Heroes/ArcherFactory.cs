using Rpg.Client.Assets.Equipments.Archer;
using Rpg.Client.Assets.GraphicConfigs.Heroes;
using Rpg.Client.Assets.Perks;
using Rpg.Client.Assets.Skills.Hero.Archer;
using Rpg.Client.Core;

namespace Rpg.Client.Assets.Heroes
{
    internal class ArcherFactory : HeroFactoryBase
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
                new AddSkillUnitLevel(1, new EnergyShotSkill()),
                new AddSkillUnitLevel(1, new ArrowRainSkill(false)),
                new AddSkillUnitLevel(2, new RapidShotSkill()),
                new AddPerkUnitLevel(3, new CriticalHit()),
                new AddSkillUnitLevel(4, new ZduhachMightSkill(false))
            };
        }
    }
}