using System.Collections.Generic;

using Rpg.Client.Core.Effects;
using Rpg.Client.Models;

namespace Rpg.Client.Core.Skills
{
    internal class MonsterAttackSkill : SkillBase
    {
        private static SkillVisualization PredefinedVisualization => new()
        {
            Type = SkillVisualizationStateType.Melee, 
            SoundEffectType = GameObjectSoundType.WolfBite
        };
        
        public MonsterAttackSkill() : base(PredefinedVisualization, false)
        {
        }

        public MonsterAttackSkill(bool costRequired) : base(PredefinedVisualization, costRequired)
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
                        DamageMultiplier = 1
                    };

                    return res;
                })
            }
        };

        public override string Sid => "Monster Attack";
        public override SkillTargetType TargetType => SkillTargetType.Enemy;
        public override SkillType Type => SkillType.Melee;
    }
}