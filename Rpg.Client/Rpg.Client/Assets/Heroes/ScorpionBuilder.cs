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
                // SkillSets = new List<SkillSet>
                // {
                //     new SkillSet
                //     {
                //         Skills = new List<SkillBase>
                //         {
                //             new SwordSlashSkill()
                //         }
                //     },
                //     new SkillSet
                //     {
                //         Skills = new List<SkillBase>
                //         {
                //             new SwordSlashSkill(),
                //             new DefenseStanceSkill(true)
                //         }
                //     },
                //     new SkillSet
                //     {
                //         Skills = new List<SkillBase>
                //         {
                //             new SwordSlashSkill(),
                //             new DefenseStanceSkill(true),
                //             new WideSlashSkill(true)
                //         }
                //     }
                // },
                UnitGraphicsConfig = new GenericCharacterGraphicsConfig()
            };
        }
    }
}