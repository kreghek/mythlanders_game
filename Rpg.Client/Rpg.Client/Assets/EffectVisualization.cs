using Rpg.Client.Core.SkillEffects;

namespace Rpg.Client.Assets
{
    internal sealed class EffectVisualization : IEffectVisualization
    {
        public int BasedOneIndex { get; init; }
    }

    internal static class EffectVisualizations
    {
        public static EffectVisualization Healing = new() { BasedOneIndex = 1 };
        public static EffectVisualization Damage = new() { BasedOneIndex = 2 };
        public static EffectVisualization Protection = new() { BasedOneIndex = 3 };
        public static EffectVisualization PowerUp = new() { BasedOneIndex = 4 };
        public static EffectVisualization PowerDown = new() { BasedOneIndex = 5 };
    }
}