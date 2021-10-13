using Microsoft.Xna.Framework.Audio;

namespace Rpg.Client.Core.Skills
{
    internal struct SkillVisualization
    {
        public SkillVisualizationStateType Type { get; set; }
    }

    internal struct SkillVisualization2
    {
        public SkillVisualization VisualizationScheme { get; set; }
        public SoundEffectInstance? UsageEffect { get; set; }
    }
}