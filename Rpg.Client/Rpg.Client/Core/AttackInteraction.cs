namespace Rpg.Client.Core
{
    internal sealed class AttackInteraction
    {
        private readonly CombatUnit _attacker;
        private readonly CombatUnit _target;
        private readonly CombatSkillCard _combatSkillCard;

        public AttackInteraction(CombatUnit attacker, CombatUnit target, CombatSkillCard combatSkillCard)
        {
            _attacker = attacker;
            _target = target;
            _combatSkillCard = combatSkillCard;
        }

        public void Execute()
        {
            _target.Unit.Hp -= _combatSkillCard.Skill.DamageMin;
        }
    }
}
