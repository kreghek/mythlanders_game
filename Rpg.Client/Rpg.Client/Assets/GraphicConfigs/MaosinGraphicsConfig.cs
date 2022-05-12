using System.Collections.Generic;

using Rpg.Client.Core;

namespace Rpg.Client.Assets.GraphicConfigs
{
    internal sealed class MaosinGraphicsConfig : UnitGraphicsConfigBase
    {
        public MaosinGraphicsConfig()
        {
            PredefinedAnimations = new Dictionary<AnimationSid, IAnimationFrameSet>
            {
                { AnimationSid.Idle, AnimationFrameSetFactory.CreateSequential(startFrameIndex: 0, frameCount: 8, speedMultiplicator: 8, isIdle: true, isLoop: true) },
                {
                    AnimationSid.MoveForward, AnimationFrameSetFactory.CreateSequential(startFrameIndex: 40, frameCount: 8, speedMultiplicator: 6)
                },
                {
                    AnimationSid.MoveBackward,
                    AnimationFrameSetFactory.CreateSequential(startFrameIndex: 40, frameCount: 8, speedMultiplicator: 8)
                },
                { AnimationSid.Skill1, AnimationFrameSetFactory.CreateSequential(startFrameIndex: 8, frameCount: 16, speedMultiplicator: 8) },
                { AnimationSid.Skill2, AnimationFrameSetFactory.CreateSequential(startFrameIndex: 24, frameCount: 8, speedMultiplicator: 8) },
                { AnimationSid.Skill3, AnimationFrameSetFactory.CreateSequential(startFrameIndex: 32, frameCount: 8, speedMultiplicator: 8) },
                { AnimationSid.Skill4, AnimationFrameSetFactory.CreateSequential(startFrameIndex: 32, frameCount: 8, speedMultiplicator: 8) },
                { AnimationSid.Wound, AnimationFrameSetFactory.CreateSequential(startFrameIndex: 48, frameCount: 8, speedMultiplicator: 8) },
                { AnimationSid.Death, AnimationFrameSetFactory.CreateSequential(startFrameIndex: 56, frameCount: 8, speedMultiplicator: 8) }
            };
        }
    }
}