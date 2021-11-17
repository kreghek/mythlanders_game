using Rpg.Client.Core.Effects;

namespace Rpg.Client.Core.Skills
{
    internal class EffectRule
    {
        public SkillDirection Direction { get; init; }

        public EffectCreator EffectCreator { get; init; }
    }
}