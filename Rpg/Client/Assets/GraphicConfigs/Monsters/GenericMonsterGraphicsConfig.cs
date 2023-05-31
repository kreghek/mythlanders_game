using System.Collections.Generic;

using Client.Assets.GraphicConfigs.Monsters;

using Rpg.Client.Core;

namespace Rpg.Client.Assets.GraphicConfigs.Monsters
{
    internal sealed class GenericMonsterGraphicsConfig : SlavicMonsterGraphicConfig
    {
        public override IDictionary<PredefinedAnimationSid, IAnimationFrameSet> GetPredefinedAnimations()
        {
            return new Dictionary<PredefinedAnimationSid, IAnimationFrameSet>
            {
                { PredefinedAnimationSid.Idle, AnimationFrameSetFactory.CreateIdle() },
                {
                    PredefinedAnimationSid.MoveForward,
                    AnimationFrameSetFactory.CreateSequential(startFrameIndex: 0, frameCount: 1, fps: 1)
                },
                {
                    PredefinedAnimationSid.MoveBackward,
                    AnimationFrameSetFactory.CreateSequential(startFrameIndex: 16, frameCount: 8, fps: 6)
                },
                {
                    PredefinedAnimationSid.Skill1,
                    AnimationFrameSetFactory.CreateSequential(startFrameIndex: 8, frameCount: 8, fps: 8)
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
                    PredefinedAnimationSid.Ult,
                    AnimationFrameSetFactory.CreateSequential(startFrameIndex: 0, frameCount: 1, fps: 1)
                },
                {
                    PredefinedAnimationSid.Wound,
                    AnimationFrameSetFactory.CreateSequential(startFrameIndex: 24, frameCount: 8, fps: 8)
                },
                {
                    PredefinedAnimationSid.Death,
                    AnimationFrameSetFactory.CreateSequential(startFrameIndex: 32, frameCount: 8, fps: 8)
                }
            };
        }
    }
}