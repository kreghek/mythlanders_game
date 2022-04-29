using System.Collections.Generic;

using Rpg.Client.Core;
using Rpg.Client.Core.SkillEffects;
using Rpg.Client.Core.Skills;
using Rpg.Client.GameScreens;
using Rpg.Client.GameScreens.Combat;

namespace Rpg.Client.Assets.Skills
{
    internal class UnholyHitSkill : VisualizedSkillBase
    {
        private const SkillSid SID = SkillSid.UnholyHit;

        public UnholyHitSkill() : this(false)
        {
        }

        public UnholyHitSkill(bool costRequired) : base(PredefinedVisualization, costRequired)
        {
        }

        public override IEnumerable<EffectRule> Rules { get; } = new[]
        {
            new EffectRule
            {
                Direction = SkillDirection.Target,
                EffectCreator = new EffectCreator(u =>
                {
                    var res = new DamageEffect
                    {
                        Actor = u,
                        DamageMultiplier = 1
                    };

                    return res;
                })
            }
        };

        public override SkillSid Sid => SID;
        public override SkillTargetType TargetType => SkillTargetType.Enemy;
        public override SkillType Type => SkillType.Melee;

        public virtual int Weight => BASE_WEIGHT * 2;

        private static SkillVisualization PredefinedVisualization => new()
        {
            AnimationSid = AnimationSid.Skill1,
            Type = SkillVisualizationStateType.Melee,
            SoundEffectType = GameObjectSoundType.SwordSlash
        };
    }
}