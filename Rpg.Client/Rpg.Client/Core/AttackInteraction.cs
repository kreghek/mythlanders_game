using System;

namespace Rpg.Client.Core
{
    internal sealed class AttackInteraction
    {
        private readonly CombatUnit _attacker;
        private readonly CombatUnit _target;
        private readonly CombatSkillCard _combatSkillCard;
        private readonly Action _postExecute;

        public AttackInteraction(CombatUnit attacker, CombatUnit target, CombatSkillCard combatSkillCard, Action postExecute)
        {
            _attacker = attacker;
            _target = target;
            _combatSkillCard = combatSkillCard;
            _postExecute = postExecute;
        }

        public void Execute()
        {
            _target.Unit.TakeDamage(_combatSkillCard.Skill.DamageMin);
            _postExecute?.Invoke();
        }
    }
}