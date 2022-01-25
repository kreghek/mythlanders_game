using Rpg.Client.Core.Equipments;
using Rpg.Client.Core.GraphicConfigs;
using Rpg.Client.Core.Perks;
using Rpg.Client.Core.Skills;

namespace Rpg.Client.Core.Heroes
{
    internal class SwordsmanBuilder : IHeroBuilder
    {
        public UnitScheme Create()
        {
            return new()
            {
                TankRank = 0.4f,
                DamageDealerRank = 0.5f,
                SupportRank = 0.1f,

                Name = UnitName.Berimir,

                Levels = new IUnitLevelScheme[]
                {
                    new AddSkillUnitLevel(1, new SwordSlashSkill()),
                    new AddSkillUnitLevel(2, new WideSlashSkill()),
                    new AddPerkUnitLevel(2, new ImprovedHitPoints()),
                    new AddSkillUnitLevel(3, new DefenseStanceSkill(true)),
                    new AddSkillUnitLevel(4, new SvarogBlastFurnaceSkill(true))
                },

                Equipments = new IEquipmentScheme[]
                {
                    new WarriorGreatSword(),
                    new Mk2MediumPowerArmor()
                },

                UnitGraphicsConfig = new BerimirGraphicsConfig()
            };
        }
    }
}