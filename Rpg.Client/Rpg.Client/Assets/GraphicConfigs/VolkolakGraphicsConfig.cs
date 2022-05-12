using System.Collections.Generic;

using Rpg.Client.Core;

namespace Rpg.Client.Assets.GraphicConfigs
{
    internal sealed class VolkolakGraphicsConfig : UnitGraphicsConfigBase
    {
        public VolkolakGraphicsConfig()
        {
            PredefinedAnimations = new Dictionary<PredefinedAnimationSid, IAnimationFrameSet>
            {
                { PredefinedAnimationSid.Idle, AnimationFrameSetFactory.CreateIdle(startFrameIndex: 40) },
                { PredefinedAnimationSid.MoveForward, AnimationFrameSetFactory.CreateSequential(startFrameIndex: 40, frameCount: 1, speedMultiplicator: 1) },
                {
                    PredefinedAnimationSid.MoveBackward, AnimationFrameSetFactory.CreateSequential(startFrameIndex: 16, frameCount: 8, speedMultiplicator: 6)
                },
                { PredefinedAnimationSid.Skill1, AnimationFrameSetFactory.CreateSequential(startFrameIndex: 56, frameCount: 8, speedMultiplicator: 8) },
                { PredefinedAnimationSid.Skill2, AnimationFrameSetFactory.CreateSequential(startFrameIndex: 0, frameCount: 1, speedMultiplicator: 1) },
                { PredefinedAnimationSid.Skill3, AnimationFrameSetFactory.CreateSequential(startFrameIndex: 0, frameCount: 1, speedMultiplicator: 1) },
                { PredefinedAnimationSid.Wound, AnimationFrameSetFactory.CreateSequential(startFrameIndex: 64, frameCount: 8, speedMultiplicator: 8) },
                { PredefinedAnimationSid.Death, AnimationFrameSetFactory.CreateSequential(startFrameIndex: 72, frameCount: 8, speedMultiplicator: 8) },
                { PredefinedAnimationSid.ShapeShift, AnimationFrameSetFactory.CreateSequential(startFrameIndex: 24, frameCount: 16, speedMultiplicator: 8) }
            };
        }
    }
}