using Rpg.Client.Core.Equipments;
using Rpg.Client.Core.GraphicConfigs;
using Rpg.Client.Core.Perks;
using Rpg.Client.Core.Skills;

namespace Rpg.Client.Core.Heroes
{
    internal class ArcherBuilder : IHeroBuilder
    {
        public UnitScheme Create()
        {
            return new()
            {
                TankRank = 0.0f,
                DamageDealerRank = 0.75f,
                SupportRank = 0.25f,

                Name = UnitName.Hawk,

                Levels = new IUnitLevelScheme[]
                {
                    new AddSkillUnitLevel(1, new EnergyShotSkill()),
                    new AddSkillUnitLevel(2, new RapidBowShotSkill()),
                    new AddPerkUnitLevel(2, new CriticalHit()),
                    new AddSkillUnitLevel(3, new ArrowRainSkill(true)),
                    new AddSkillUnitLevel(4, new DefenseStanceSkill(true))
                },

                Equipments = new IEquipmentScheme[]
                {
                    new ArcherPulsarBow(),
                    new Mk3ScoutPowerArmor()
                },

                UnitGraphicsConfig = new HawkGraphicsConfig()
            };
        }
    }
}