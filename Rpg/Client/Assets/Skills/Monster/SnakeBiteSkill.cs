using Rpg.Client.Core;
using Rpg.Client.Core.Skills;
using Rpg.Client.GameScreens;

namespace Rpg.Client.Assets.Skills.Monster
{
    internal class SnakeBiteSkill : MonsterAttackSkill
    {
        public SnakeBiteSkill() : base(PredefinedVisualization, false)
        {
        }

        private static SkillVisualization PredefinedVisualization => new()
        {
            Type = SkillVisualizationStateType.Melee,
            SoundEffectType = GameObjectSoundType.AspidBite,
            AnimationSid = PredefinedAnimationSid.Skill1
        };
    }
}