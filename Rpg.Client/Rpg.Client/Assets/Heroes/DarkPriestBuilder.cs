using Rpg.Client.Assets.Equipments;
using Rpg.Client.Assets.Perks;
using Rpg.Client.Assets.Skills;
using Rpg.Client.Core;
using Rpg.Client.Core.GraphicConfigs;

namespace Rpg.Client.Assets.Heroes
{
    internal class DarkPriestBuilder : IHeroBuilder
    {
        public UnitName UnitName { get; }

        public UnitScheme Create(IBalanceTable balanceTable)
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
                    new AddSkillUnitLevel(3, new ParalyticChoirSkill(true)),
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