using Rpg.Client.Assets.Equipments;
using Rpg.Client.Assets.Perks;
using Rpg.Client.Assets.Skills;
using Rpg.Client.Core;

namespace Rpg.Client.Assets.Heroes
{
    internal class LegionnaireFactory : HeroFactoryBase
    {
        protected override IEquipmentScheme[] GetEquipment()
        {
            return new IEquipmentScheme[]
            {
                new EmberGladius(),
                new Mk2MediumPowerArmor2(),
                new BrokenAresSculpture()
            };
        }

        protected override IUnitLevelScheme[] GetLevels()
        {
            return new IUnitLevelScheme[]
            {
                new AddSkillUnitLevel(1, new SwordSwingSkill()),
                new AddSkillUnitLevel(2, new ShieldSkill()),
                new AddPerkUnitLevel(2, new ImprovedHitPoints()),
                new AddSkillUnitLevel(3, new JavelinThrowSkill(true)),
                new AddSkillUnitLevel(4, new AresWarBringerThreadsSkill(true))
            };
        }

        public override UnitName HeroName => UnitName.Leonidas;
    }
}