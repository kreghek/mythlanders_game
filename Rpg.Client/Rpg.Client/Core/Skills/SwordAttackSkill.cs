namespace Rpg.Client.Core.Skills
{
    internal class SwordAttackSkill : AttackSkillBase
    {
        public override SkillScope Scope => SkillScope.Single;
        public override SkillType Type => SkillType.Melee;
    }
}