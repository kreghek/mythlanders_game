using System;

namespace Rpg.Client.Core
{
    internal sealed class AttackInteraction : IUnitInteraction
    {
        private readonly ActiveCombat _combat;
        private readonly CombatUnit _attacker;
        private readonly CombatSkillCard _combatSkillCard;
        private readonly Action _postExecute;
        private readonly CombatUnit _target;

        public AttackInteraction(ActiveCombat combat, CombatUnit attacker, CombatUnit target, CombatSkillCard combatSkillCard,
            Action postExecute)
        {
            _combat = combat;
            _attacker = attacker;
            _target = target;
            _combatSkillCard = combatSkillCard;
            _postExecute = postExecute;
        }

        public void Execute()
        {
            _combat.UseSkill(_combatSkillCard.Skill, _target);
            _postExecute?.Invoke();
        }
    }
}