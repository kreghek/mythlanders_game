using Rpg.Client.Assets.Equipments.Medjay;
using Rpg.Client.Assets.GraphicConfigs;
using Rpg.Client.Assets.Perks;
using Rpg.Client.Assets.Skills.Hero.Medjay;
using Rpg.Client.Core;

namespace Rpg.Client.Assets.Heroes
{
    internal class MedjayFactory : HeroFactoryBase
    {
        public override UnitName HeroName => UnitName.Medjay;

        protected override IEquipmentScheme[] GetEquipment()
        {
            return new IEquipmentScheme[]
            {
                new UltraLightSaber(),
                new FireResistBlackArmor(),
                new GreenTattoo()
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
                new AddSkillUnitLevel(1, new PoisonCutSkill()),
                new AddSkillUnitLevel(1, new SuperNaturalAgilitySkill(false)),

                new AddSkillUnitLevel(2, new SunburstSkill(false)),

                new AddPerkUnitLevel(3, new CriticalHit()),

                new AddSkillUnitLevel(4, new ChorusEyeSkill(false))
            };
        }
    }
}