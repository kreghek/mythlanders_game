using Rpg.Client.Core;
using Rpg.Client.Core.Skills;
using Rpg.Client.GameScreens;

namespace Rpg.Client.Assets.Skills.Monster
{
    internal class WolfBiteSkill : MonsterAttackSkill
    {
        public WolfBiteSkill() : base(PredefinedVisualization, false)
        {
        }

        private static SkillVisualization PredefinedVisualization => new()
        {
            Type = SkillVisualizationStateType.Melee,
            SoundEffectType = GameObjectSoundType.WolfBite,
            AnimationSid = PredefinedAnimationSid.Skill1
        };
    }
}