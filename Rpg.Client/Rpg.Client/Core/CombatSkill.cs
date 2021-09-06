namespace Rpg.Client.Core
{
    public class CombatSkill
    {
        public int DamageMax { get; set; }
        public int DamageMaxPerLevel { get; set; }
        public int DamageMin { get; set; }
        public int DamageMinPerLevel { get; set; }

        public SkillScope Scope { get; set; }

        public SkillTarget TargetType { get; set; }
    }
}