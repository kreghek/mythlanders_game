namespace Rpg.Client.Core.Skills
{
    internal class BowAttackSkill : AttackSkillBase
    {
        public override SkillScope Scope => SkillScope.Single;
        public override SkillType Type => SkillType.Range;
    }
}