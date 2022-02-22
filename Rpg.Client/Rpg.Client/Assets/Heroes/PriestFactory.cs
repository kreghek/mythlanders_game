using Rpg.Client.Assets.Equipments;
using Rpg.Client.Assets.Perks;
using Rpg.Client.Assets.Skills;
using Rpg.Client.Core;

namespace Rpg.Client.Assets.Heroes
{
    internal class PriestFactory : HeroFactoryBase
    {
        protected override IEquipmentScheme[] GetEquipment()
        {
            return new IEquipmentScheme[]
            {
                new EgyptianBookOfDeath(),
                new NanoMetalLongCloths(),
                new ScarabeusKingLeg()
            };
        }

        protected override IUnitLevelScheme[] GetLevels()
        {
            return new IUnitLevelScheme[]
            {
                new AddSkillUnitLevel(1, new DarkLightingSkill()),
                new AddSkillUnitLevel(2, new MummificationTouchSkill()),
                new AddPerkUnitLevel(2, new Evasion()),
                new AddSkillUnitLevel(3, new ParalyticChoirSkill(true)),
                new AddSkillUnitLevel(4, new FingerOfAnubisShotSkill(true))
            };
        }

        public override UnitName HeroName => UnitName.Kakhotep;
    }
}