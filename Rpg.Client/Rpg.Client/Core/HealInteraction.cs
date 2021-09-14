using System;

namespace Rpg.Client.Core
{
    internal sealed class HealInteraction
    {
        private readonly ActiveCombat _combat;
        private readonly CombatSkillCard _combatSkillCard;
        private readonly CombatUnit _healer;
        private readonly Action _postExecute;
        private readonly CombatUnit _target;

        public HealInteraction(ActiveCombat combat, CombatUnit healer, CombatUnit target,
            CombatSkillCard combatSkillCard,
            Action postExecute)
        {
            _combat = combat;
            _healer = healer;
            _target = target;
            _combatSkillCard = combatSkillCard;
            _postExecute = postExecute;
        }

        public void Execute()
        {
            _combat.UseSkill(_combatSkillCard.Skill, _target);
            //_target.Unit.TakeHeal(_combatSkillCard.Skill.DamageMin);
            _postExecute?.Invoke();
        }
    }
}