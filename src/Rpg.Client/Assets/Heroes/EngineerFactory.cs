using Rpg.Client.Assets.Equipments.Engineer;
using Rpg.Client.Assets.Perks;
using Rpg.Client.Assets.Skills.Hero.Engineer;
using Rpg.Client.Core;

namespace Rpg.Client.Assets.Heroes
{
    internal class EngineerFactory : HeroFactoryBase
    {
        public override UnitName HeroName => UnitName.Engineer;

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
                new AddSkillUnitLevel<FlameThrowingSkill>(1),
                new AddSkillUnitLevel<PipeBludgeonSkill>(2),
                new AddPerkUnitLevel<ImprovedArmor>(2),
                new AddSkillUnitLevel<DismantlementSkill>(3),
                new AddSkillUnitLevel<CouosLegacySkill>(4)
            };
        }
    }
}