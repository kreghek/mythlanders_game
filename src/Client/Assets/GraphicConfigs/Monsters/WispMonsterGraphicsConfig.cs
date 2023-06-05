using System.Collections.Generic;

using Client.Assets.GraphicConfigs.Monsters;

using Rpg.Client.Core;

namespace Rpg.Client.Assets.GraphicConfigs.Monsters
{
    internal sealed class WispMonsterGraphicsConfig : SlavicMonsterGraphicConfig
    {
        public WispMonsterGraphicsConfig(UnitName unit) : base(unit)
        {
        }

        public override IDictionary<PredefinedAnimationSid, IAnimationFrameSet> GetPredefinedAnimations()
        {
            return new Dictionary<PredefinedAnimationSid, IAnimationFrameSet>
            {
                { PredefinedAnimationSid.Idle, AnimationFrameSetFactory.CreateIdle(fps: 12) },
                {
                    PredefinedAnimationSid.MoveForward,
                    AnimationFrameSetFactory.CreateSequential(startFrameIndex: 0, frameCount: 1, fps: 1)
                },
                {
                    PredefinedAnimationSid.MoveBackward,
                    AnimationFrameSetFactory.CreateSequential(startFrameIndex: 0, frameCount: 1, fps: 1)
                },
                {
                    PredefinedAnimationSid.Skill1,
                    AnimationFrameSetFactory.CreateSequential(startFrameIndex: 8, frameCount: 8, fps: 12)
                },
                {
                    PredefinedAnimationSid.Skill2,
                    AnimationFrameSetFactory.CreateSequential(startFrameIndex: 0, frameCount: 1, fps: 1)
                },
                {
                    PredefinedAnimationSid.Skill3,
                    AnimationFrameSetFactory.CreateSequential(startFrameIndex: 0, frameCount: 1, fps: 1)
                },
                {
                    PredefinedAnimationSid.Wound,
                    AnimationFrameSetFactory.CreateSequential(startFrameIndex: 16, frameCount: 2, fps: 8)
                },
                {
                    PredefinedAnimationSid.Death,
                    AnimationFrameSetFactory.CreateSequential(startFrameIndex: 24, frameCount: 8, fps: 8)
                }
            };
        }
    }
}