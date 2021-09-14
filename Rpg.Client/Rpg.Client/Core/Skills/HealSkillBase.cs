namespace Rpg.Client.Core.Skills
{
    internal abstract class HealSkillBase : SkillBase
    {
        public override SkillDirection Direction => SkillDirection.Friendly;
        public int HealMax { get; set; }
        public int HealMaxPerLevel { get; set; }
        public int HealMin { get; set; }
        public int HealMinPerLevel { get; set; }
    }
}