namespace Rpg.Client.Core.Skills
{
    internal abstract class AttackSkillBase : SkillBase
    {
        public int DamageMax { get; set; }
        public int DamageMaxPerLevel { get; set; }
        public int DamageMin { get; set; }
        public int DamageMinPerLevel { get; set; }
        public override SkillDirection Direction => SkillDirection.Enemy;
    }
}