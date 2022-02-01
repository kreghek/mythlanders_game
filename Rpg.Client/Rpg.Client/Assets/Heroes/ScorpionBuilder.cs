using Rpg.Client.Assets.Equipments;
using Rpg.Client.Assets.Perks;
using Rpg.Client.Assets.Skills;
using Rpg.Client.Core;
using Rpg.Client.Core.GraphicConfigs;

namespace Rpg.Client.Assets.Heroes
{
    internal class ScorpionBuilder : IHeroBuilder
    {
        public UnitScheme Create()
        {
            return new()
            {
                TankRank = 0.1f,
                DamageDealerRank = 0.8f,
                SupportRank = 0.1f,

                Name = UnitName.Amun,

                Levels = new IUnitLevelScheme[]
                {
                    new AddSkillUnitLevel(1, new SwordSlashSkill()),
                    new AddSkillUnitLevel(2, new PoisonedSpearSkill()),
                    new AddPerkUnitLevel(2, new CriticalHit()),
                    new AddSkillUnitLevel(3, new SuperNaturalAgilitySkill(true)),
                    new AddSkillUnitLevel(4, new SunburstSkill(true)),
                },

                Equipments = new IEquipmentScheme[]
                {
                    new TribalEquipment(),
                    new FireResistBlackArmor(),
                    new GreenTattoo()
                },

                UnitGraphicsConfig = new GenericCharacterGraphicsConfig()
            };
        }
    }
}