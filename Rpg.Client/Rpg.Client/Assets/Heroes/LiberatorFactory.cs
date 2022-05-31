using Rpg.Client.Assets.Equipments.Liberator;
using Rpg.Client.Assets.Perks;
using Rpg.Client.Assets.Skills;
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
                new AddSkillUnitLevel(1, new MotivationSkill()),
                new AddSkillUnitLevel(2, new FightAgainstMastersSkill()),
                new AddPerkUnitLevel(2, new Evasion()),
                new AddSkillUnitLevel(3, new BraveHeartsSkill(true)),
                new AddSkillUnitLevel(4, new LiberationSkill(true))
            };
        }
    }
}