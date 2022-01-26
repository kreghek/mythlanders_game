using Rpg.Client.Core.Equipments;
using Rpg.Client.Core.GraphicConfigs;
using Rpg.Client.Core.Perks;
using Rpg.Client.Core.Skills;

namespace Rpg.Client.Core.Heroes
{
    internal class HerbalistBuilder : IHeroBuilder
    {
        public UnitScheme Create()
        {
            return new()
            {
                TankRank = 0.0f,
                DamageDealerRank = 0.0f,
                SupportRank = 1.0f,

                Name = UnitName.Rada,

                Levels = new IUnitLevelScheme[]
                {
                    new AddSkillUnitLevel(1, new HealingSalveSkill()),
                    new AddSkillUnitLevel(2, new ToxicHerbsSkill()),
                    new AddPerkUnitLevel(2, new CriticalHeal()),
                    new AddSkillUnitLevel(3, new DopeHerbSkill(true)),
                    new AddSkillUnitLevel(4, new MassHealSkill(true))
                },
                
                Equipments = new IEquipmentScheme[]
                {
                    new HerbBag(),
                    new WomanShort(),
                    new BookOfHerbs()
                },
                
                UnitGraphicsConfig = new GenericCharacterGraphicsConfig()
            };
        }
    }
}