using Rpg.Client.Assets.Equipments;
using Rpg.Client.Assets.Perks;
using Rpg.Client.Assets.Skills;
using Rpg.Client.Core;

namespace Rpg.Client.Assets.Heroes
{
    internal class ScorpionBuilder : HeroFactoryBase
    {
        protected override IEquipmentScheme[] GetEquipment()
        {
            return new IEquipmentScheme[]
            {
                new TribalEquipment(),
                new FireResistBlackArmor(),
                new GreenTattoo()
            };
        }

        protected override IUnitLevelScheme[] GetLevels()
        {
            return new IUnitLevelScheme[]
            {
                new AddSkillUnitLevel(1, new SwordSlashSkill()),
                new AddSkillUnitLevel(2, new PoisonedSpearSkill()),
                new AddPerkUnitLevel(2, new CriticalHit()),
                new AddSkillUnitLevel(3, new SuperNaturalAgilitySkill(true)),
                new AddSkillUnitLevel(4, new SunburstSkill(true))
            };
        }

        public override UnitName HeroName => UnitName.Amun;
    }
}