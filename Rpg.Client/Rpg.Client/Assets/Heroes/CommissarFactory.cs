using Rpg.Client.Assets.Equipments.Sergeant;
using Rpg.Client.Assets.GraphicConfigs;
using Rpg.Client.Assets.Perks;
using Rpg.Client.Assets.Skills.Hero.Commissar;
using Rpg.Client.Core;

namespace Rpg.Client.Assets.Heroes
{
    internal class CommissarFactory : HeroFactoryBase
    {
        public override UnitName HeroName => UnitName.Thar;

        protected override IEquipmentScheme[] GetEquipment()
        {
            return new IEquipmentScheme[]
            {
                new CompactSaber(),
                new RedMediumPowerArmor(),
                new MultifunctionalClocks()
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
                new AddSkillUnitLevel(1, new InspiringRushSkill()),
                new AddSkillUnitLevel(1, new TacticalManeuverSkill()),
                new AddSkillUnitLevel(2, new BlankShotSkill()),
                new AddPerkUnitLevel(3, new Evasion())
            };
        }
    }
}