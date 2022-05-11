using Rpg.Client.Core.SkillEffects;

namespace Rpg.Client.Assets.Skills.Hero.Assaulter
{
    internal sealed class AssaultSkillRuleMetadata : ISkillEffectMetadata
    {
        public bool IsShot { get; set; }
        public bool IsBuff { get; internal set; }
    }
}