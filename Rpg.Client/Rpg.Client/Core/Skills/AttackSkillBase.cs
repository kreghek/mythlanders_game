namespace Rpg.Client.Core.Skills
{
    internal abstract class AttackSkillBase : SkillBase
    {
        public override SkillDirection Direction => SkillDirection.Enemy;
        public int DamageMax { get; set; }
        public int DamageMaxPerLevel { get; set; }
        public int DamageMin { get; set; }
        public int DamageMinPerLevel { get; set; }
    }
}