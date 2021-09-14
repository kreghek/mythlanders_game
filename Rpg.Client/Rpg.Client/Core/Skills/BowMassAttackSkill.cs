namespace Rpg.Client.Core.Skills
{
    internal class BowMassAttackSkill : AttackSkillBase
    {
        public override SkillScope Scope => SkillScope.Mass;
        public override SkillType Type => SkillType.Range;
    }
}