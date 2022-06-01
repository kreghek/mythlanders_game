using Rpg.Client.Assets.Equipments.Legionnaire;
using Rpg.Client.Assets.GraphicConfigs;
using Rpg.Client.Assets.Perks;
using Rpg.Client.Assets.Skills;
using Rpg.Client.Core;

namespace Rpg.Client.Assets.Heroes
{
    internal class HopliteFactory : HeroFactoryBase
    {
        public override UnitName HeroName => UnitName.Hoplite;

        protected override IEquipmentScheme[] GetEquipment()
        {
            return new IEquipmentScheme[]
            {
                new EmberDori(),
                new EmpoweredMk2MediumPowerArmor(),
                new BrokenAresSculpture()
            };
        }

        protected override IUnitLevelScheme[] GetLevels()
        {
            return new IUnitLevelScheme[]
            {
                new AddSkillUnitLevel(1, new OffensiveSkill()),
                new AddSkillUnitLevel(1, new PhalanxSkill()),
                new AddPerkUnitLevel(3, new ImprovedHitPoints()),
                new AddSkillUnitLevel(3, new JavelinThrowSkill(true)),
                new AddSkillUnitLevel(4, new AresWarBringerThreadsSkill(true))
            };
        }

        protected override UnitGraphicsConfigBase GetGraphicsConfig()
        {
            return new SingleSpriteGraphicsConfig();
        }
    }
}