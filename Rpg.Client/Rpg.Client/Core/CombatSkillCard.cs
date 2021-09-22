using System;

using Rpg.Client.Core.Skills;

namespace Rpg.Client.Core
{
    internal class CombatSkillCard
    {
        private readonly ICombatSkillContext _combatSkillContext;

        public CombatSkillCard(SkillBase skill, ICombatSkillContext combatSkillContext)
        {
            Skill = skill ?? throw new ArgumentNullException(nameof(skill));
            _combatSkillContext = combatSkillContext;
        }

        public bool IsAvailable
        {
            get
            {
                if (Skill.Cost is not null)
                {
                    var currentMana = _combatSkillContext.GetMana();
                    return currentMana >= Skill.Cost;
                }

                return true;
            }
        }

        public SkillBase Skill { get; }
    }

    internal interface ICombatSkillContext
    {
        int GetMana();
    }

    internal sealed class CombatSkillContext : ICombatSkillContext
    {
        private readonly CombatUnit _combatUnit;

        public CombatSkillContext(CombatUnit combatUnit)
        {
            _combatUnit = combatUnit;
        }

        public int GetMana()
        {
            return _combatUnit.Unit.ManaPool;
        }
    }
}