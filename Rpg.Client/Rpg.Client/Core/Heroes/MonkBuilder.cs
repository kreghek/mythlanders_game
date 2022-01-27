using Rpg.Client.Assets.Perks;
using Rpg.Client.Assets.Skills;
using Rpg.Client.Core.Equipments;
using Rpg.Client.Core.GraphicConfigs;
using Rpg.Client.Core.Skills;

namespace Rpg.Client.Core.Heroes
{
    internal class MonkBuilder : IHeroBuilder
    {
        public UnitScheme Create()
        {
            return new()
            {
                TankRank = 0.2f,
                DamageDealerRank = 0.6f,
                SupportRank = 0.2f,

                Name = UnitName.Maosin,

                Levels = new IUnitLevelScheme[]
                {
                    new AddSkillUnitLevel(1, new StaffHitSkill()),
                    new AddSkillUnitLevel(2, new HealingMantreSkill()),
                    new AddPerkUnitLevel(2, new Evasion()),
                    new AddSkillUnitLevel(3, new MasterStaffHitSkill(true)),
                    new AddSkillUnitLevel(4, new GodNatureSkill(true))
                },

                Equipments = new IEquipmentScheme[]
                {
                    new RedemptionStaff(),
                    new AsceticRobe(),
                    new SymbolOfGod()
                },

                UnitGraphicsConfig = new MaosinGraphicsConfig()
            };
        }
    }
}