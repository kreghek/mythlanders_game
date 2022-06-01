using Rpg.Client.Core.SkillEffects;

namespace Rpg.Client.Core.Skills
{
    internal class EffectRule
    {
        public ITargetSelector Direction { get; init; }

        public EffectCreator EffectCreator { get; init; }

        public ISkillEffectMetadata? EffectMetadata { get; set; }
    }
}