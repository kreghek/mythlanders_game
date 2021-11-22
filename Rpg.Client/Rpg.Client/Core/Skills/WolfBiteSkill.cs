using Rpg.Client.GameScreens;

namespace Rpg.Client.Core.Skills
{
    internal class WolfBiteSkill : MonsterAttackSkill
    {
        private static SkillVisualization PredefinedVisualization => new()
        {
            Type = SkillVisualizationStateType.Melee,
            SoundEffectType = GameObjectSoundType.WolfBite
        };
        
        public WolfBiteSkill() : base(PredefinedVisualization, false)
        {
        }
    }
}