using System.Collections.Generic;

using Rpg.Client.Core;

namespace Rpg.Client.Assets.GraphicConfigs
{
    internal sealed class VolkolakGraphicsConfig : UnitGraphicsConfigBase
    {
        public VolkolakGraphicsConfig()
        {
            PredefinedAnimations = new Dictionary<AnimationSid, IAnimationFrameSet>
            {
                { AnimationSid.Idle, AnimationFrameSetFactory.CreateIdle(startFrameIndex: 40) },
                { AnimationSid.MoveForward, AnimationFrameSetFactory.CreateSequential(startFrameIndex: 40, frameCount: 1, speedMultiplicator: 1) },
                {
                    AnimationSid.MoveBackward, AnimationFrameSetFactory.CreateSequential(startFrameIndex: 16, frameCount: 8, speedMultiplicator: 6)
                },
                { AnimationSid.Skill1, AnimationFrameSetFactory.CreateSequential(startFrameIndex: 56, frameCount: 8, speedMultiplicator: 8) },
                { AnimationSid.Skill2, AnimationFrameSetFactory.CreateSequential(startFrameIndex: 0, frameCount: 1, speedMultiplicator: 1) },
                { AnimationSid.Skill3, AnimationFrameSetFactory.CreateSequential(startFrameIndex: 0, frameCount: 1, speedMultiplicator: 1) },
                { AnimationSid.Wound, AnimationFrameSetFactory.CreateSequential(startFrameIndex: 64, frameCount: 8, speedMultiplicator: 8) },
                { AnimationSid.Death, AnimationFrameSetFactory.CreateSequential(startFrameIndex: 72, frameCount: 8, speedMultiplicator: 8) },
                { AnimationSid.ShapeShift, AnimationFrameSetFactory.CreateSequential(startFrameIndex: 24, frameCount: 16, speedMultiplicator: 8) }
            };
        }
    }
}