namespace Rpg.Client.Core.Skills
{
    internal class SweepingAttackSkill : AttackSkillBase
    {
        public override SkillScope Scope => SkillScope.Mass;
        public override SkillType Type => SkillType.Melee;
    }
}