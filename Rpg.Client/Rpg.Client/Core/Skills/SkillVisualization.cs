using Rpg.Client.Models;

namespace Rpg.Client.Core.Skills
{
    internal struct SkillVisualization
    {
        public SkillVisualizationStateType Type { get; init; }
        public GameObjectSoundType SoundEffectType { get; init; }
    }
}