using Rpg.Client.Assets.Equipments;
using Rpg.Client.Assets.Perks;
using Rpg.Client.Assets.Skills;
using Rpg.Client.Core;
using Rpg.Client.Core.GraphicConfigs;

namespace Rpg.Client.Assets.Heroes
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
                    new CombatSword(),
                    new Mk2MediumPowerArmor(),
                    new WoodenHandSculpture()
                },

                UnitGraphicsConfig = new BerimirGraphicsConfig()
            };
        }
    }
}