using Rpg.Client.Models;

namespace Rpg.Client.Core.Skills
{
    internal class WolfBiteSkill : MonsterAttackSkill
    {
        public WolfBiteSkill() : base(new SkillVisualization
        { Type = SkillVisualizationStateType.Melee, SoundEffectType = GameObjectSoundType.WolfBite }, false)
        {
        }
    }

    internal class BearBludgeonSkill : MonsterAttackSkill
    {
        public BearBludgeonSkill() : base(new SkillVisualization
        { Type = SkillVisualizationStateType.Melee, SoundEffectType = GameObjectSoundType.BearBludgeon }, false)
        {
        }
    }
}