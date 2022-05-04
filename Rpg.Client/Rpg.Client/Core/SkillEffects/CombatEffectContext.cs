using Rpg.Client.Core.Skills;

namespace Rpg.Client.Core.SkillEffects
{
    internal class CombatEffectContext
    {
        public CombatEffectContext(ICombat combat, ISkill sourceSkill)
        {
            Combat = combat;
            SourceSkill = sourceSkill;
        }

        public ICombat Combat { get; }
        public ISkill SourceSkill { get; }
    }
}