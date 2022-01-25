using Rpg.Client.Core.GraphicConfigs;

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

                // SkillSets = new List<SkillSet>
                // {
                //     new SkillSet
                //     {
                //         Skills = new List<SkillBase>
                //         {
                //             new BowShotSkill()
                //         }
                //     },
                //     new SkillSet
                //     {
                //         Skills = new List<SkillBase>
                //         {
                //             new BowShotSkill(),
                //             new MassStunSkill(true)
                //         }
                //     },
                //     new SkillSet
                //     {
                //         Skills = new List<SkillBase>
                //         {
                //             new BowShotSkill(),
                //             new MassStunSkill(true),
                //             new SwordSlashSkill(true) // Finger of the Anubis
                //         }
                //     }
                // },
                UnitGraphicsConfig = new GenericCharacterGraphicsConfig()
            };
        }
    }
}