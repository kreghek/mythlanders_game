using Rpg.Client.Assets.Equipments.Amazon;
using Rpg.Client.Assets.Perks;
using Rpg.Client.Assets.Skills;
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

        protected override IUnitLevelScheme[] GetLevels()
        {
            return new IUnitLevelScheme[]
            {
                new AddSkillUnitLevel(1, new ShotOfHateSkill()),
                new AddSkillUnitLevel(2, new PainfulWoundSkill()),
                new AddPerkUnitLevel(2, new CriticalHit()),
                new AddSkillUnitLevel(3, new WarCrySkill(true)),
                new AddSkillUnitLevel(4, new TribeDefenderSkill(true))
            };
        }
    }
}