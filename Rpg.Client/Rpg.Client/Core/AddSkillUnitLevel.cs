using Rpg.Client.Core.Skills;

namespace Rpg.Client.Core
{
    internal sealed class AddSkillUnitLevel : UnitLevelBase
    {
        private readonly ISkill _skill;

        public AddSkillUnitLevel(int level, ISkill skill) : base(level)
        {
            _skill = skill;
        }

        public override void Apply(Unit unit)
        {
            unit.Skills.Add(_skill);
        }
    }
}