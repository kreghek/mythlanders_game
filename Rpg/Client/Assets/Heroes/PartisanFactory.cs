using Rpg.Client.Assets.Equipments.Sergeant;
using Rpg.Client.Assets.GraphicConfigs;
using Rpg.Client.Assets.Perks;
using Rpg.Client.Assets.Skills.Hero.Commissar;
using Rpg.Client.Core;

namespace Rpg.Client.Assets.Heroes
{
    internal class PartisanFactory : HeroFactoryBase
    {
        public override UnitName HeroName => UnitName.Comissar;

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
            return new SingleSpriteGraphicsConfig();
        }

        protected override IUnitLevelScheme[] GetLevels()
        {
            return new IUnitLevelScheme[]
            {
                new AddSkillUnitLevel<InspiringRushSkill>(1),
                new AddSkillUnitLevel<TacticalManeuverSkill>(1),
                new AddSkillUnitLevel<BlankShotSkill>(2),
                new AddPerkUnitLevel<Evasion>(3)
            };
        }
    }
}