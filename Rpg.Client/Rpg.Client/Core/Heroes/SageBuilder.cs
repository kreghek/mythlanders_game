using Rpg.Client.Core.GraphicConfigs;

namespace Rpg.Client.Core.Heroes
{
    internal class SageBuilder : IHeroBuilder
    {
        public UnitScheme Create()
        {
            return new()
            {
                TankRank = 0.2f,
                DamageDealerRank = 0.0f,
                SupportRank = 0.8f,

                Name = UnitName.Cheng,

                // SkillSets = new List<SkillSet>
                // {
                //     new SkillSet
                //     {
                //         Skills = new List<SkillBase>
                //         {
                //             new DopeHerbSkill()
                //         }
                //     },
                //     new SkillSet
                //     {
                //         Skills = new List<SkillBase>
                //         {
                //             new DopeHerbSkill(),
                //             new PowerUpSkill(true)
                //         }
                //     },
                //     new SkillSet
                //     {
                //         Skills = new List<SkillBase>
                //         {
                //             new DopeHerbSkill(), // No violence, please
                //             new PowerUpSkill(true), // Trust
                //             new HealSkill(true) // God Merciful Touch
                //         }
                //     }
                // },
                UnitGraphicsConfig = new GenericCharacterGraphicsConfig()
            };
        }
    }
}