using Rpg.Client.Core.SkillEffects;

namespace Rpg.Client.Assets
{
    internal sealed class EffectVisualization: IEffectVisualization
    {
        public int BasedOneIndex { get; init; }
    }

    internal static class EffectVisualizations
    {
        public static EffectVisualization Healing = new EffectVisualization{ BasedOneIndex = 1 };
        public static EffectVisualization Damage = new EffectVisualization{ BasedOneIndex = 2 };
        public static EffectVisualization Protection = new EffectVisualization{ BasedOneIndex = 3 };
        public static EffectVisualization PowerUp = new EffectVisualization{ BasedOneIndex = 4 };
    }
}