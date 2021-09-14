namespace Rpg.Client.Core.Skills
{
    internal abstract class SkillBase
    {
        public abstract SkillDirection Direction { get; }
        public abstract SkillScope Scope { get; }
        public abstract SkillType Type { get; }
    }
}