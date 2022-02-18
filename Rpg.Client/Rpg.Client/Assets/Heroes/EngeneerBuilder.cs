using Rpg.Client.Assets.Equipments;
using Rpg.Client.Assets.Perks;
using Rpg.Client.Assets.Skills;
using Rpg.Client.Core;
using Rpg.Client.Core.GraphicConfigs;

namespace Rpg.Client.Assets.Heroes
{
    internal class EngeneerBuilder : IHeroBuilder
    {
        public UnitName HeroName { get; }

        public UnitScheme Create(IBalanceTable balanceTable)
        {
            return new(balanceTable.GetCommonUnitBasics())
            {
                TankRank = 0.25f,
                DamageDealerRank = 0.5f,
                SupportRank = 0.25f,

                Name = UnitName.Geron,

                Levels = new IUnitLevelScheme[]
                {
                    new AddSkillUnitLevel(1, new FlameThrowingSkill()),
                    new AddSkillUnitLevel(2, new PipeBludgeonSkill()),
                    new AddPerkUnitLevel(2, new ImprovedArmor()),
                    new AddSkillUnitLevel(3, new DemountageSkill(true)),
                    new AddSkillUnitLevel(4, new CouosLegacySkill(true))
                },

                Equipments = new IEquipmentScheme[]
                {
                    new FlameThrower(),
                    new HeavyCooperHandmadeArmor(),
                    new ScientificTableOfMaterials()
                },

                UnitGraphicsConfig = new HawkGraphicsConfig()
            };
        }
    }
}