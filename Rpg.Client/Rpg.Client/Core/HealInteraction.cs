using System;

namespace Rpg.Client.Core
{
    internal sealed class HealInteraction
    {
        private readonly CombatUnit _healer;
        private readonly CombatSkillCard _combatSkillCard;
        private readonly Action _postExecute;
        private readonly CombatUnit _target;

        public HealInteraction(CombatUnit healer, CombatUnit target, CombatSkillCard combatSkillCard,
            Action postExecute)
        {
            _healer = healer;
            _target = target;
            _combatSkillCard = combatSkillCard;
            _postExecute = postExecute;
        }

        public void Execute()
        {
            _target.Unit.TakeHeal(_combatSkillCard.Skill.DamageMin);
            _postExecute?.Invoke();
        }
    }
}