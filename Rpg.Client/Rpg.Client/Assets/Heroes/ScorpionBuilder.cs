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
                },

                UnitGraphicsConfig = new GenericCharacterGraphicsConfig()
            };
        }
    }
}