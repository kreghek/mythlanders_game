using System.Collections.Generic;

using Rpg.Client.Core;

namespace Rpg.Client.Assets.GraphicConfigs
{
    internal sealed class SpearmanGraphicsConfig : UnitGraphicsConfigBase
    {
        public SpearmanGraphicsConfig()
        {
            PredefinedAnimations = new Dictionary<PredefinedAnimationSid, IAnimationFrameSet>
            {
                { PredefinedAnimationSid.Idle, AnimationFrameSetFactory.CreateIdle() },
                { PredefinedAnimationSid.Defense, AnimationFrameSetFactory.CreateSequential(startFrameIndex: 39, frameCount: 1, speedMultiplicator: 0) },
                {
                    PredefinedAnimationSid.MoveForward, AnimationFrameSetFactory.CreateSequential(startFrameIndex: 32, frameCount: 8, speedMultiplicator: 6)
                },
                {
                    PredefinedAnimationSid.MoveBackward,
                    AnimationFrameSetFactory.CreateSequential(startFrameIndex: 32, frameCount: 8, speedMultiplicator: 6)
                },

                { PredefinedAnimationSid.Skill1, AnimationFrameSetFactory.CreateSequential(startFrameIndex: 8, frameCount: 8, speedMultiplicator: 8) },
                { PredefinedAnimationSid.Skill2, AnimationFrameSetFactory.CreateSequential(startFrameIndex: 16, frameCount: 8, speedMultiplicator: 8) },
                { PredefinedAnimationSid.Skill3, AnimationFrameSetFactory.CreateSequential(startFrameIndex: 24, frameCount: 8, speedMultiplicator: 8) },
                { PredefinedAnimationSid.Skill4, AnimationFrameSetFactory.CreateSequential(startFrameIndex: 32, frameCount: 8, speedMultiplicator: 8) },

                { PredefinedAnimationSid.Ult, AnimationFrameSetFactory.CreateSequential(startFrameIndex: 59, frameCount: 2, speedMultiplicator: 8) },

                { PredefinedAnimationSid.Wound, AnimationFrameSetFactory.CreateSequential(startFrameIndex: 40, frameCount: 8, speedMultiplicator: 8) },
                { PredefinedAnimationSid.Death, AnimationFrameSetFactory.CreateSequential(startFrameIndex: 48, frameCount: 8, speedMultiplicator: 8) }
            };
        }
    }
}