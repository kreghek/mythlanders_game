using Client.Core;

using Rpg.Client.Core.Skills;

namespace Rpg.Client.Core.SkillEffects
{
    internal class CombatEffectContext
    {
        public CombatEffectContext(ICombat combat, IEffectSource effectSource)
        {
            Combat = combat;
            EffectSource = effectSource;
        }

        public ICombat Combat { get; }
        public IEffectSource EffectSource { get; }
    }
}