using Rpg.Client.Assets.Equipments;
using Rpg.Client.Assets.Perks;
using Rpg.Client.Assets.Skills;
using Rpg.Client.Core;
using Rpg.Client.Core.GraphicConfigs;

namespace Rpg.Client.Assets.Heroes
{
    internal class LegionnaireBuilder : IHeroBuilder
    {
        public UnitName UnitName { get; }

        public UnitScheme Create(IBalanceTable balanceTable)
        {
            return new()
            {
                TankRank = 0.4f,
                DamageDealerRank = 0.5f,
                SupportRank = 0.1f,

                Name = UnitName.Leonidas,

                Levels = new IUnitLevelScheme[]
                {
                    new AddSkillUnitLevel(1, new SwordSwingSkill()),
                    new AddSkillUnitLevel(2, new ShieldSkill()),
                    new AddPerkUnitLevel(2, new ImprovedHitPoints()),
                    new AddSkillUnitLevel(3, new JavelinThrowSkill(true)),
                    new AddSkillUnitLevel(4, new AresWarBringerThreadsSkill(true))
                },

                Equipments = new IEquipmentScheme[]
                {
                    new EmberGladius(),
                    new Mk2MediumPowerArmor2(),
                    new BrokenAresSculpture()
                },

                UnitGraphicsConfig = new BerimirGraphicsConfig()
            };
        }
    }
}