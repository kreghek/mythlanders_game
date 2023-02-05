using Rpg.Client.Assets.Equipments.Amazon;
using Rpg.Client.Assets.GraphicConfigs;
using Rpg.Client.Assets.Perks;
using Rpg.Client.Assets.Skills.Hero.Amazon;
using Rpg.Client.Core;

namespace Rpg.Client.Assets.Heroes
{
    internal class AmazonFactory : HeroFactoryBase
    {
        public override UnitName HeroName => UnitName.Amazon;

        protected override IEquipmentScheme[] GetEquipment()
        {
            return new IEquipmentScheme[]
            {
                new HunterRifle(),
                new TribeHunterScoutArmor(),
                new TheClawKnife()
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
                new AddSkillUnitLevel<ShotOfHateSkill>(1),
                new AddSkillUnitLevel<PainfulWoundSkill>(2),
                new AddPerkUnitLevel<CriticalHit>(2),
                new AddSkillUnitLevel<WarCrySkill>(3),
                new AddSkillUnitLevel<TribeDefenderSkill>(4)
            };
        }
    }
}