namespace Rpg.Client.Core
{
    internal class CombatSkill
    {
        public int DamageMax { get; set; }
        public int DamageMaxPerLevel { get; set; }
        public int DamageMin { get; set; }
        public int DamageMinPerLevel { get; set; }

        public SkillScope Scope { get; set; }

        public SkillTarget TargetType { get; set; }
    }
}