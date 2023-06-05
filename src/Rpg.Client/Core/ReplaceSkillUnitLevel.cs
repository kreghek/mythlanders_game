using System.Linq;

using Rpg.Client.Core.Skills;

namespace Rpg.Client.Core
{
    internal sealed class ReplaceSkillUnitLevel : UnitLevelBase
    {
        private readonly ISkill _newSkill;
        private readonly SkillSid _targetSid;

        public ReplaceSkillUnitLevel(int level, SkillSid targetSid, ISkill newSkill) : base(level)
        {
            _targetSid = targetSid;
            _newSkill = newSkill;
        }

        public override void Apply(Unit unit)
        {
            var targetSkill = unit.Skills.Single(x => x.Sid == _targetSid);
            var index = unit.Skills.IndexOf(targetSkill);
            unit.Skills[index] = _newSkill;
        }
    }
}