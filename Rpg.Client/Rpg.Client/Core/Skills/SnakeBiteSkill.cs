using Rpg.Client.Models;

namespace Rpg.Client.Core.Skills
{
    internal class SnakeBiteSkill : MonsterAttackSkill
    {
        public SnakeBiteSkill() : base(new SkillVisualization
            { Type = SkillVisualizationStateType.Melee, SoundEffectType = GameObjectSoundType.SnakeBite }, false)
        {
        }
    }
}