using System.Collections.Generic;

using Rpg.Client.Core.Effects;
using Rpg.Client.Models;

namespace Rpg.Client.Core.Skills
{
    internal class BowShotSkill : SkillBase
    {
        public BowShotSkill() : this(false)
        {
        }

        public BowShotSkill(bool costRequired) : base(new SkillVisualization
        { Type = SkillVisualizationStateType.Range, SoundEffectType = GameObjectSoundType.BowShot }, costRequired)
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
                        PowerMultiplier = 1.5f,
                        ValueRange = 1
                    };

                    return res;
                })
            }
        };

        public override string Sid => "Strike";
        public override SkillTargetType TargetType => SkillTargetType.Enemy;
        public override SkillType Type => SkillType.Range;
    }
}