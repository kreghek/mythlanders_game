using Rpg.Client.Assets.Equipments.Scorpion;
using Rpg.Client.Assets.Perks;
using Rpg.Client.Assets.Skills.Hero.Scorpion;
using Rpg.Client.Core;

namespace Rpg.Client.Assets.Heroes
{
    internal class ScorpionFactory : HeroFactoryBase
    {
        public override UnitName HeroName => UnitName.Scorpion;

        protected override IEquipmentScheme[] GetEquipment()
        {
            return new IEquipmentScheme[]
            {
                new UltraLightSpear(),
                new FireResistBlackArmor(),
                new GreenTattoo()
            };
        }

        protected override IUnitLevelScheme[] GetLevels()
        {
            return new IUnitLevelScheme[]
            {
                new AddSkillUnitLevel(2, new PoisonedSpearSkill()),
                new AddPerkUnitLevel(2, new CriticalHit()),
                new AddSkillUnitLevel(3, new SuperNaturalAgilitySkill(true)),
                new AddSkillUnitLevel(4, new SunburstSkill(true))
            };
        }
    }
}