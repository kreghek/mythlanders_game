using System;

namespace Rpg.Client.Core
{
    internal sealed class AttackInteraction : IUnitInteraction
    {
        private readonly CombatSkillCard _combatSkillCard;
        private readonly Action _postExecute;
        private readonly CombatUnit _target;

        public AttackInteraction(CombatUnit target, CombatSkillCard combatSkillCard, Action postExecute)
        {
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