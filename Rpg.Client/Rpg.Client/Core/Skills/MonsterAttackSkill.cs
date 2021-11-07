using System.Collections.Generic;

using Rpg.Client.Core.Effects;
using Rpg.Client.Models;

namespace Rpg.Client.Core.Skills
{
    internal class MonsterAttackSkill : SkillBase
    {
        public MonsterAttackSkill() : base(new SkillVisualization
        { Type = SkillVisualizationStateType.Melee, SoundEffectType = GameObjectSoundType.WolfBite }, false)
        {
        }

        public MonsterAttackSkill(bool costRequired) : base(new SkillVisualization
        { Type = SkillVisualizationStateType.Melee, SoundEffectType = GameObjectSoundType.WolfBite }, costRequired)
        {
        }

        protected MonsterAttackSkill(SkillVisualization visualization, bool costRequired) : base(visualization,
            costRequired)
        {
        }

        public override IEnumerable<EffectRule> Rules { get; } = new List<EffectRule>
        {
            new EffectRule
            {
                Direction = SkillDirection.Target,
                EffectCreator = new EffectCreator(u =>
                {
                    var res = new AttackEffect
                    {
                        Actor = u,
                        PowerMultiplier = 1,
                        ValueRange = 1
                    };

                    return res;
                })
            }
        };

        public override string Sid => "Monster Attack";
        public override SkillTargetType TargetType => SkillTargetType.Enemy;
        public override SkillType Type => SkillType.Melee;
    }

    internal class WolfBiteSkill : MonsterAttackSkill
    {
        public WolfBiteSkill() : base(new SkillVisualization
        { Type = SkillVisualizationStateType.Melee, SoundEffectType = GameObjectSoundType.WolfBite }, false)
        {
        }
    }

    internal class SnakeBiteSkill : MonsterAttackSkill
    {
        public SnakeBiteSkill() : base(new SkillVisualization
        { Type = SkillVisualizationStateType.Melee, SoundEffectType = GameObjectSoundType.SnakeBite }, false)
        {
        }
    }
}