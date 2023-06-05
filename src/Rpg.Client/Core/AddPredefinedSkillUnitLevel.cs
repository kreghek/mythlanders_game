using Rpg.Client.Core.Skills;

namespace Rpg.Client.Core
{
    internal sealed class AddPredefinedSkillUnitLevel : UnitLevelBase
    {
        private readonly ISkill _skill;

        public AddPredefinedSkillUnitLevel(int level, ISkill skill) : base(level)
        {
            _skill = skill;
        }

        public override void Apply(Unit unit)
        {
            unit.Skills.Add(_skill);
        }
    }
}