using System.Collections.Generic;

using Rpg.Client.Core;

namespace Rpg.Client.Assets.GraphicConfigs
{
    internal sealed class SingleSpriteGraphicsConfig : UnitGraphicsConfigBase
    {
        public override IDictionary<PredefinedAnimationSid, IAnimationFrameSet> GetPredefinedAnimations()
        {
            return new Dictionary<PredefinedAnimationSid, IAnimationFrameSet>
            {
                {
                    PredefinedAnimationSid.Idle,
                    AnimationFrameSetFactory.CreateIdle(startFrameIndex: 0, frameCount: 1, fps: 1)
                },
                {
                    PredefinedAnimationSid.Defense,
                    AnimationFrameSetFactory.CreateSequential(startFrameIndex: 0, frameCount: 1, fps: 1)
                },

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
                    AnimationFrameSetFactory.CreateSequential(startFrameIndex: 0, frameCount: 1, fps: 1)
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
                    PredefinedAnimationSid.Skill4,
                    AnimationFrameSetFactory.CreateSequential(startFrameIndex: 0, frameCount: 1, fps: 1)
                },

                {
                    PredefinedAnimationSid.ShapeShift,
                    AnimationFrameSetFactory.CreateSequential(startFrameIndex: 0, frameCount: 1, fps: 1)
                },
                {
                    PredefinedAnimationSid.Wound,
                    AnimationFrameSetFactory.CreateSequential(startFrameIndex: 0, frameCount: 1, fps: 1)
                },
                {
                    PredefinedAnimationSid.Death,
                    AnimationFrameSetFactory.CreateSequential(startFrameIndex: 1000, frameCount: 1, fps: 1)
                }
            };
        }
    }
}