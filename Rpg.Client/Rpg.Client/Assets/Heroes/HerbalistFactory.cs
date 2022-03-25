using Rpg.Client.Assets.Equipments;
using Rpg.Client.Assets.Perks;
using Rpg.Client.Assets.Skills;
using Rpg.Client.Core;

namespace Rpg.Client.Assets.Heroes
{
    internal class HerbalistFactory : HeroFactoryBase
    {
        public override UnitName HeroName => UnitName.Rada;

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
                new AddSkillUnitLevel(2, new ToxicGasSkill()),
                new AddPerkUnitLevel(2, new CriticalHeal()),
                new AddSkillUnitLevel(3, new DopeHerbSkill(true)),
                new AddSkillUnitLevel(4, new MassHealSkill(true))
            };
        }
    }
}