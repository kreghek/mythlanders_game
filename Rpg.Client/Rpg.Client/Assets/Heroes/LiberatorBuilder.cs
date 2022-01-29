using Rpg.Client.Assets.Equipments;
using Rpg.Client.Assets.Perks;
using Rpg.Client.Assets.Skills;
using Rpg.Client.Core;
using Rpg.Client.Core.GraphicConfigs;

namespace Rpg.Client.Assets.Heroes
{
    internal class LiberatorBuilder : IHeroBuilder
    {
        public UnitScheme Create()
        {
            return new()
            {
                TankRank = 0.1f,
                DamageDealerRank = 0.9f,
                SupportRank = 0.0f,

                Name = UnitName.Nubiti,

                Levels = new IUnitLevelScheme[]
                {
                    new AddSkillUnitLevel(1, new MotivationSkill()),
                    new AddSkillUnitLevel(2, new FightAgainsMastersSkill()),
                    new AddPerkUnitLevel(2, new Evasion()),
                    new AddSkillUnitLevel(3, new BraveHeartsSkill(true)),
                    new AddSkillUnitLevel(4, new FingerOfAnubisShotSkill(true))
                },

                Equipments = new IEquipmentScheme[]
                {
                    new VoiceModulator(),
                    new HiddenExoskeleton(),
                    new NewLawCodexOfFreedom()
                },

                UnitGraphicsConfig = new GenericCharacterGraphicsConfig()
            };
        }
    }
}