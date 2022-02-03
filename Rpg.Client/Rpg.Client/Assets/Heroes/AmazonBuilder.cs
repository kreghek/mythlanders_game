using Rpg.Client.Assets.Equipments;
using Rpg.Client.Assets.Perks;
using Rpg.Client.Assets.Skills;
using Rpg.Client.Core;
using Rpg.Client.Core.GraphicConfigs;

namespace Rpg.Client.Assets.Heroes
{
    internal class AmazonBuilder : IHeroBuilder
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
                    new AddSkillUnitLevel(1, new ShotOfHateSkill()),
                    new AddSkillUnitLevel(2, new PainfullWoundSkill()),
                    new AddPerkUnitLevel(2, new CriticalHit()),
                    new AddSkillUnitLevel(3, new WarCrySkill(true)),
                    new AddSkillUnitLevel(4, new TribeDefenderSkill(true))
                },

                Equipments = new IEquipmentScheme[]
                {
                    new ArcherPulsarBow2(),
                    new Mk3ScoutPowerArmor2(),
                    new OldShiningGem()
                },

                UnitGraphicsConfig = new GenericCharacterGraphicsConfig()
            };
        }
    }
}