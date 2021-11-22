using Rpg.Client.GameScreens;

namespace Rpg.Client.Core.Skills
{
    internal class SnakeBiteSkill : MonsterAttackSkill
    {
        private static SkillVisualization PredefinedVisualization => new()
        {
            Type = SkillVisualizationStateType.Melee,
            SoundEffectType = GameObjectSoundType.SnakeBite
        };

        public SnakeBiteSkill() : base(PredefinedVisualization, false)
        {
        }
    }
}