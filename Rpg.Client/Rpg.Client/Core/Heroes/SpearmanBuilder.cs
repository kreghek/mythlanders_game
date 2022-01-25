using Rpg.Client.Core.GraphicConfigs;

namespace Rpg.Client.Core.Heroes
{
    internal class SpearmanBuilder : IHeroBuilder
    {
        public UnitScheme Create()
        {
            return new()
            {
                TankRank = 0.8f,
                DamageDealerRank = 0.1f,
                SupportRank = 0.1f,

                Name = UnitName.Ping,
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