using Rpg.Client.Assets.Equipments;
using Rpg.Client.Assets.GraphicConfigs;
using Rpg.Client.Assets.Perks;
using Rpg.Client.Assets.Skills;
using Rpg.Client.Core;

namespace Rpg.Client.Assets.Heroes
{
    internal class DullFactory : HeroFactoryBase
    {
        public override UnitName HeroName => UnitName.Dull;

        protected override IEquipmentScheme[] GetEquipment()
        {
            return new IEquipmentScheme[]
            {
                new CombatSword(),
                new Mk2MediumPowerArmor(),
                new WoodenHandSculpture()
            };
        }

        protected override UnitGraphicsConfigBase GetGraphicsConfig()
        {
            return new SingleSpriteMonsterGraphicsConfig();
        }

        protected override IUnitLevelScheme[] GetLevels()
        {
            return new IUnitLevelScheme[]
            {
                new AddSkillUnitLevel(1, new ShotgunSkill()),
                new AddSkillUnitLevel(1, new DefenseStanceSkill(true)),
                new AddSkillUnitLevel(2, new WideSlashSkill(true)),
                new AddPerkUnitLevel(2, new ImprovedHitPoints()),
                new AddSkillUnitLevel(3, new GroupProtectionSkill(true)),
                new AddSkillUnitLevel(4, new SvarogBlastFurnaceSkill(true))
            };
        }
    }
}