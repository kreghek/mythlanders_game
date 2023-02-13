using Rpg.Client.Assets.Equipments.Sage;
using Rpg.Client.Assets.Perks;
using Rpg.Client.Assets.Skills.Hero.Sage;
using Rpg.Client.Core;

namespace Rpg.Client.Assets.Heroes
{
    internal class SageFactory : HeroFactoryBase
    {
        public override UnitName HeroName => UnitName.Sage;

        protected override IEquipmentScheme[] GetEquipment()
        {
            return new IEquipmentScheme[]
            {
                new EmptinessInTheHand(),
                new DeceptivelyLightRobe(),
                new MagicAndMechanicalBox()
            };
        }

        protected override IUnitLevelScheme[] GetLevels()
        {
            return new IUnitLevelScheme[]
            {
                new AddSkillUnitLevel<ReproachSkill>(1),
                new AddSkillUnitLevel<NoViolencePleaseSkill>(2),
                new AddPerkUnitLevel<Evasion>(2),
                new AddSkillUnitLevel<FaithBoostSkill>(3),
                new AddSkillUnitLevel<AskedNoViolenceSkill>(4)
            };
        }
    }
}