using Rpg.Client.Assets.Equipments.Liberator;
using Rpg.Client.Assets.Perks;
using Rpg.Client.Assets.Skills.Hero.Liberator;
using Rpg.Client.Core;

namespace Rpg.Client.Assets.Heroes
{
    internal class LiberatorFactory : HeroFactoryBase
    {
        public override UnitName HeroName => UnitName.Liberator;

        protected override IEquipmentScheme[] GetEquipment()
        {
            return new IEquipmentScheme[]
            {
                new VoiceModulator(),
                new HiddenExoskeleton(),
                new NewLawCodexOfFreedom()
            };
        }

        protected override IUnitLevelScheme[] GetLevels()
        {
            return new IUnitLevelScheme[]
            {
                new AddSkillUnitLevel<MotivationSkill>(1),
                new AddSkillUnitLevel<FightAgainstMastersSkill>(2),
                new AddPerkUnitLevel<Evasion>(2),
                new AddSkillUnitLevel<BraveHeartsSkill>(3),
                new AddSkillUnitLevel<LiberationSkill>(4)
            };
        }
    }
}