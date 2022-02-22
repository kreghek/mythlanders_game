using Rpg.Client.Assets.Equipments;
using Rpg.Client.Assets.Perks;
using Rpg.Client.Assets.Skills;
using Rpg.Client.Core;

namespace Rpg.Client.Assets.Heroes
{
    internal class SpearmanFactory : HeroFactoryBase
    {
        protected override IEquipmentScheme[] GetEquipment()
        {
            return new IEquipmentScheme[]
            {
                new EliteGuardsmanSpear(),
                new JuggernautHeavyPowerArmor(),
                new ChaoticNeuroInterface()
            };
        }

        protected override IUnitLevelScheme[] GetLevels()
        {
            return new IUnitLevelScheme[]
            {
                new AddSkillUnitLevel(1, new PenetrationStrikeSkill()),
                new AddSkillUnitLevel(2, new StonePathSkill()),
                new AddPerkUnitLevel(2, new ImprovedArmor()),
                new AddSkillUnitLevel(3, new DemonicTauntSkill(true)),
                new AddSkillUnitLevel(4, new ToateAngerSkill(true))
            };
        }

        public override UnitName HeroName => UnitName.Ping;
    }
}