using Rpg.Client.Assets.Equipments.Swordsman;
using Rpg.Client.Assets.GraphicConfigs.Heroes;
using Rpg.Client.Assets.Perks;
using Rpg.Client.Assets.Skills.Hero.Swordsman;
using Rpg.Client.Core;

namespace Rpg.Client.Assets.Heroes
{
    internal class SwordsmanFactory : HeroFactoryBase
    {
        public override UnitName HeroName => UnitName.Swordsman;

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
            return new SwordsmanGraphicsConfig();
        }

        protected override IUnitLevelScheme[] GetLevels()
        {
            return new IUnitLevelScheme[]
            {
                new AddSkillUnitLevel<SwordSlashSkill>(1),
                new AddSkillUnitLevel<DefenseStanceSkill>(1),
                new AddSkillUnitLevel<WideSlashSkill>(2),
                new AddPerkUnitLevel<ImprovedHitPoints>(3),
                new AddSkillUnitLevel<SvarogBlastFurnaceSkill>(4)
            };
        }
    }
}