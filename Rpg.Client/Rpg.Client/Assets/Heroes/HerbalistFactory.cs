using Rpg.Client.Assets.Equipments.Herbalist;
using Rpg.Client.Assets.Perks;
using Rpg.Client.Assets.Skills.Hero.Herbalist;
using Rpg.Client.Core;

namespace Rpg.Client.Assets.Heroes
{
    internal class HerbalistFactory : HeroFactoryBase
    {
        public override UnitName HeroName => UnitName.Herbalist;

        protected override IEquipmentScheme[] GetEquipment()
        {
            return new IEquipmentScheme[]
            {
                new HerbBag(),
                new WomanShort(),
                new BookOfHerbs()
            };
        }

        protected override IUnitLevelScheme[] GetLevels()
        {
            return new IUnitLevelScheme[]
            {
                new AddSkillUnitLevel(1, new HealingSalveSkill()),
                new AddSkillUnitLevel(1, new ToxicGasSkill()),
                new AddSkillUnitLevel(2, new DopeHerbSkill(true)),
                new AddPerkUnitLevel(3, new CriticalHeal()),
                new AddSkillUnitLevel(4, new MassHealSkill(true))
            };
        }
    }
}