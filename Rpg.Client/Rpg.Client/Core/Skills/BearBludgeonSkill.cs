using Rpg.Client.GameScreens;

namespace Rpg.Client.Core.Skills
{
    internal class BearBludgeonSkill : MonsterAttackSkill
    {
        public BearBludgeonSkill() : base(PredefinedVisualization, false)
        {
        }

        private static SkillVisualization PredefinedVisualization => new()
        {
            Type = SkillVisualizationStateType.Melee,
            SoundEffectType = GameObjectSoundType.BearBludgeon
        };
    }
}