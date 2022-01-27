using Rpg.Client.Assets.Perks;
using Rpg.Client.Assets.Skills;
using Rpg.Client.Core.Equipments;
using Rpg.Client.Core.GraphicConfigs;
using Rpg.Client.Core.Skills;

namespace Rpg.Client.Core.Heroes
{
    internal class DarkPriestBuilder : IHeroBuilder
    {
        public UnitScheme Create()
        {
            return new()
            {
                TankRank = 0.1f,
                DamageDealerRank = 0.9f,
                SupportRank = 0.0f,

                Name = UnitName.Kakhotep,

                Levels = new IUnitLevelScheme[]
                {
                    new AddSkillUnitLevel(1, new DarkLightingSkill()),
                    new AddSkillUnitLevel(2, new MummificationTouchSkill()),
                    new AddPerkUnitLevel(2, new Evasion()),
                    new AddSkillUnitLevel(3, new ParaliticChoirSkill(true)),
                    new AddSkillUnitLevel(4, new FingerOfAnubisShotSkill(true))
                },

                Equipments = new IEquipmentScheme[]
                {
                    new EgyptianBookOfDeath(),
                    new NanoMetalLongCloths(),
                    new ScarabeusKingLeg()
                },

                UnitGraphicsConfig = new GenericCharacterGraphicsConfig()
            };
        }
    }
}