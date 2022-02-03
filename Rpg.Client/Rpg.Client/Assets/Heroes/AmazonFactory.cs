using Rpg.Client.Assets.Equipments;
using Rpg.Client.Assets.Perks;
using Rpg.Client.Assets.Skills;
using Rpg.Client.Core;

namespace Rpg.Client.Assets.Heroes
{
    internal class AmazonFactory : HeroFactoryBase
    {
        public override UnitName UnitName => UnitName.Diana;

        protected override IEquipmentScheme[] GetEquipment()
        {
            return new IEquipmentScheme[]
            {
                new ArcherPulsarBow2(),
                new Mk3ScoutPowerArmor2(),
                new OldShiningGem()
            };
        }

        protected override IUnitLevelScheme[] GetLevels()
        {
            return new IUnitLevelScheme[]
            {
                new AddSkillUnitLevel(1, new ShotOfHateSkill()),
                new AddSkillUnitLevel(2, new PainfullWoundSkill()),
                new AddPerkUnitLevel(2, new CriticalHit()),
                new AddSkillUnitLevel(3, new WarCrySkill(true)),
                new AddSkillUnitLevel(4, new TribeDefenderSkill(true))
            };
        }
    }
}