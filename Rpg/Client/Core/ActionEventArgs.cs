using System;

using Rpg.Client.Core.SkillEffects;
using Rpg.Client.Core.Skills;

namespace Rpg.Client.Core
{
    internal class ActionEventArgs : EventArgs
    {
        public ActionEventArgs(SkillExecution action, ICombatUnit actor, ISkill skill, ICombatUnit target)
        {
            Action = action;
            Actor = actor;
            Skill = skill;
            Target = target;
        }

        public SkillExecution Action { get; }
        public ICombatUnit Actor { get; }
        public ISkill Skill { get; }
        public ICombatUnit Target { get; }
    }
}