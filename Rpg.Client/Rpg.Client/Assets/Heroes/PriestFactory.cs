using Rpg.Client.Assets.Equipments.Priest;
using Rpg.Client.Assets.Perks;
using Rpg.Client.Assets.Skills.Hero.Priest;
using Rpg.Client.Core;

namespace Rpg.Client.Assets.Heroes
{
    internal class PriestFactory : HeroFactoryBase
    {
        public override UnitName HeroName => UnitName.Priest;

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
                new AddSkillUnitLevel<DarkLightingSkill>(1),
                new AddSkillUnitLevel<MummificationTouchSkill>(2),
                new AddPerkUnitLevel<Evasion>(2),
                new AddSkillUnitLevel<ParalyticChoirSkill>(3),
                new AddSkillUnitLevel<FingerOfAnubisShotSkill>(4)
            };
        }
    }
}