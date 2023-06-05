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
                new AddSkillUnitLevel<PoisonCutSkill>(1),
                new AddSkillUnitLevel<SuperNaturalAgilitySkill>(1),

                new AddSkillUnitLevel<SunburstSkill>(2),

                new AddPerkUnitLevel<CriticalHit>(3),

                new AddSkillUnitLevel<ChorusEyeSkill>(4)
            };
        }
    }
}