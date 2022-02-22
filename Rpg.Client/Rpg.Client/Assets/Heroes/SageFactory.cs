using Rpg.Client.Assets.Equipments;
using Rpg.Client.Assets.Perks;
using Rpg.Client.Assets.Skills;
using Rpg.Client.Core;

namespace Rpg.Client.Assets.Heroes
{
    internal class SageFactory : HeroFactoryBase
    {
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
                new AddSkillUnitLevel(1, new ReproachSkill()),
                new AddSkillUnitLevel(2, new NoViolencePleaseSkill()),
                new AddPerkUnitLevel(2, new Evasion()),
                new AddSkillUnitLevel(3, new FaithBoostSkill(true)),
                new AddSkillUnitLevel(4, new AskedNoViolenceSkill(true))
            };
        }

        public override UnitName HeroName => UnitName.Cheng;
    }
}