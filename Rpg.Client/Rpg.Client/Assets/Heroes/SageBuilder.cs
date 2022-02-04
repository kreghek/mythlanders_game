using Rpg.Client.Assets.Equipments;
using Rpg.Client.Assets.Perks;
using Rpg.Client.Assets.Skills;
using Rpg.Client.Core;
using Rpg.Client.Core.GraphicConfigs;

namespace Rpg.Client.Assets.Heroes
{
    internal class SageBuilder : IHeroBuilder
    {
        public UnitName UnitName { get; }

        public UnitScheme Create(IBalanceTable balanceTable)
        {
            return new()
            {
                TankRank = 0.2f,
                DamageDealerRank = 0.1f,
                SupportRank = 0.7f,

                Name = UnitName.Cheng,

                Levels = new IUnitLevelScheme[]
                {
                    new AddSkillUnitLevel(1, new ReproachSkill()),
                    new AddSkillUnitLevel(2, new NoViolencePleaseSkill()),
                    new AddPerkUnitLevel(2, new Evasion()),
                    new AddSkillUnitLevel(3, new FaithBoostSkill(true)),
                    new AddSkillUnitLevel(4, new AskedNoViolenceSkill(true))
                },

                Equipments = new IEquipmentScheme[]
                {
                    new EmptinessInTheHand(),
                    new DeceptivelyLightRobe(),
                    new MagicAndMechanicalBox()
                },

                UnitGraphicsConfig = new GenericCharacterGraphicsConfig()
            };
        }
    }
}