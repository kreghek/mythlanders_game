using Rpg.Client.Assets.Equipments;
using Rpg.Client.Assets.GraphicConfigs;
using Rpg.Client.Assets.Perks;
using Rpg.Client.Assets.Skills.Hero.Sergeant;
using Rpg.Client.Core;

namespace Rpg.Client.Assets.Heroes
{
    internal class SergantFactory : HeroFactoryBase
    {
        public override UnitName HeroName => UnitName.Thar;

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
                new AddSkillUnitLevel(1, new BlankShotSkill()),
                new AddPerkUnitLevel(1, new Evasion()),
                new AddSkillUnitLevel(1, new InspiringRushSkill())
            };
        }
    }
}