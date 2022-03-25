using Rpg.Client.Assets.Equipments;
using Rpg.Client.Assets.Perks;
using Rpg.Client.Assets.Skills;
using Rpg.Client.Core;

namespace Rpg.Client.Assets.Heroes
{
    internal class EngineerFactory : HeroFactoryBase
    {
        public override UnitName HeroName => UnitName.Geron;

        protected override IEquipmentScheme[] GetEquipment()
        {
            return new IEquipmentScheme[]
            {
                new FlameThrower(),
                new HeavyCooperHandmadeArmor(),
                new ScientificTableOfMaterials()
            };
        }

        protected override IUnitLevelScheme[] GetLevels()
        {
            return new IUnitLevelScheme[]
            {
                new AddSkillUnitLevel(1, new FlameThrowingSkill()),
                new AddSkillUnitLevel(2, new PipeBludgeonSkill()),
                new AddPerkUnitLevel(2, new ImprovedArmor()),
                new AddSkillUnitLevel(3, new DemountageSkill(true)),
                new AddSkillUnitLevel(4, new CouosLegacySkill(true))
            };
        }
    }
}