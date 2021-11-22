using System;

using Rpg.Client.Core.Skills;

namespace Rpg.Client.Core
{
    internal class ActionEventArgs : EventArgs
    {
        public ActionEventArgs(Action action, CombatUnit actor, ISkill skill, CombatUnit target)
        {
            Action = action;
            Actor = actor;
            Skill = skill;
            Target = target;
        }

        public Action Action { get; }
        public CombatUnit Actor { get; }
        public ISkill Skill { get; }
        public CombatUnit Target { get; }
    }
}