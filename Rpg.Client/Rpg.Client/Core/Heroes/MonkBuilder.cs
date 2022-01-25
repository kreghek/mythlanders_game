using Rpg.Client.Core.GraphicConfigs;

namespace Rpg.Client.Core.Heroes
{
    internal class MonkBuilder : IHeroBuilder
    {
        public UnitScheme Create()
        {
            return new()
            {
                TankRank = 0.2f,
                DamageDealerRank = 0.6f,
                SupportRank = 0.2f,

                Name = UnitName.Maosin,
                // SkillSets = new List<SkillSet>
                // {
                //     new SkillSet
                //     {
                //         Skills = new List<SkillBase>
                //         {
                //             new StaffSkill()
                //         }
                //     },
                //     new SkillSet
                //     {
                //         Skills = new List<SkillBase>
                //         {
                //             new StaffSkill(),
                //             new DefenseStanceSkill(true)
                //         }
                //     },
                //     new SkillSet
                //     {
                //         Skills = new List<SkillBase>
                //         {
                //             new StaffSkill(),
                //             new DefenseStanceSkill(true),
                //             new WideSlashSkill(true)
                //         }
                //     }
                // },
                UnitGraphicsConfig = new MaosinGraphicsConfig()
            };
        }
    }
}