using Rpg.Client.Assets.Perks;
using Rpg.Client.Assets.Skills;
using Rpg.Client.Core.Equipments;
using Rpg.Client.Core.GraphicConfigs;
using Rpg.Client.Core.Skills;

namespace Rpg.Client.Core.Heroes
{
    internal class SpearmanBuilder : IHeroBuilder
    {
        public UnitScheme Create()
        {
            return new()
            {
                TankRank = 0.8f,
                DamageDealerRank = 0.1f,
                SupportRank = 0.1f,

                Name = UnitName.Ping,

                Levels = new IUnitLevelScheme[]
                {
                    new AddSkillUnitLevel(1, new PenetrationStrikeSkill()),
                    new AddSkillUnitLevel(2, new StonePathSkill()),
                    new AddPerkUnitLevel(2, new ImprovedArmor()),
                    new AddSkillUnitLevel(3, new DemonicTauntSkill(true)),
                    new AddSkillUnitLevel(4, new ToateAngerSkill(true))
                },

                Equipments = new IEquipmentScheme[]
                {
                    new EliteGuardsmanSpear(),
                    new JuggernautHeavyPowerArmor(),
                    new ChaoticNeuroInterface()
                },

                UnitGraphicsConfig = new GenericCharacterGraphicsConfig()
            };
        }
    }
}