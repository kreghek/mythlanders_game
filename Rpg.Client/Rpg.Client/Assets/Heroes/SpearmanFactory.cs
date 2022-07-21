using Rpg.Client.Assets.Equipments.Spearman;
using Rpg.Client.Assets.GraphicConfigs.Heroes;
using Rpg.Client.Assets.Perks;
using Rpg.Client.Assets.Skills.Hero.Spearman;
using Rpg.Client.Core;

namespace Rpg.Client.Assets.Heroes
{
    internal class SpearmanFactory : HeroFactoryBase
    {
        public override UnitName HeroName => UnitName.Spearman;

        protected override IEquipmentScheme[] GetEquipment()
        {
            return new IEquipmentScheme[]
            {
                new EliteGuardsmanSpear(),
                new JuggernautHeavyPowerArmor(),
                new ChaoticNeuroInterface()
            };
        }

        protected override UnitGraphicsConfigBase GetGraphicsConfig()
        {
            return new SpearmanGraphicsConfig();
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
    }
}