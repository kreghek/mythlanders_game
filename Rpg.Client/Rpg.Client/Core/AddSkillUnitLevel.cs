using System;

using Rpg.Client.Core.Skills;

namespace Rpg.Client.Core
{
    internal sealed class AddSkillUnitLevel<TSkill> : UnitLevelBase where TSkill : ISkill, new()
    {
        public AddSkillUnitLevel(int level) : base(level)
        {
        }

        public override void Apply(Unit unit)
        {
            var skill = Activator.CreateInstance<TSkill>();
            unit.Skills.Add(skill);
        }
    }
}