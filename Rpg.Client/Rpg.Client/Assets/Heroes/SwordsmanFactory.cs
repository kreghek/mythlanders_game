using Rpg.Client.Assets.Equipments;
using Rpg.Client.Assets.GraphicConfigs;
using Rpg.Client.Assets.Perks;
using Rpg.Client.Assets.Skills;
using Rpg.Client.Core;

namespace Rpg.Client.Assets.Heroes
{
    internal class SwordsmanFactory : HeroFactoryBase
    {
        public override UnitName HeroName => UnitName.Berimir;

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
            return new BerimirGraphicsConfig();
        }

        protected override IUnitLevelScheme[] GetLevels()
        {
            return new IUnitLevelScheme[]
            {
                new AddSkillUnitLevel(1, new SwordSlashSkill()),
                new AddSkillUnitLevel(2, new WideSlashSkill()),
                new AddPerkUnitLevel(2, new ImprovedHitPoints()),
                new AddSkillUnitLevel(3, new DefenseStanceSkill(true)),
                new AddSkillUnitLevel(4, new SvarogBlastFurnaceSkill(true))
            };
        }
    }
}