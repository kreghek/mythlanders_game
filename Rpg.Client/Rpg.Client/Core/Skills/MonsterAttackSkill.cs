using System.Collections.Generic;

using Rpg.Client.Core.SkillEffects;
using Rpg.Client.GameScreens;

namespace Rpg.Client.Core.Skills
{
    internal class MonsterAttackSkill : SkillBase
    {
        public MonsterAttackSkill() : base(PredefinedVisualization, false)
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

        public override SkillSid Sid => SkillSid.AbstractMonsterAttack;
        public override SkillTargetType TargetType => SkillTargetType.Enemy;
        public override SkillType Type => SkillType.Melee;

        private static SkillVisualization PredefinedVisualization => new()
        {
            Type = SkillVisualizationStateType.Melee,
            SoundEffectType = GameObjectSoundType.WolfBite
        };
    }
}