using System.Collections.Generic;

using Rpg.Client.Core;

namespace Rpg.Client.Assets.GraphicConfigs
{
    internal sealed class GenericMonsterGraphicsConfig : UnitGraphicsConfigBase
    {
        public GenericMonsterGraphicsConfig()
        {
            PredefinedAnimations = new Dictionary<AnimationSid, IAnimationFrameSet>
            {
                { AnimationSid.Idle, AnimationFrameSetFactory.CreateIdle() },
                { AnimationSid.MoveForward, AnimationFrameSetFactory.CreateSequential(startFrameIndex: 0, frameCount: 1, speedMultiplicator: 1) },
                {
                    AnimationSid.MoveBackward, AnimationFrameSetFactory.CreateSequential(startFrameIndex: 16, frameCount: 8, speedMultiplicator: 6)
                },
                { AnimationSid.Skill1, AnimationFrameSetFactory.CreateSequential(startFrameIndex: 8, frameCount: 8, speedMultiplicator: 8) },
                { AnimationSid.Skill2, AnimationFrameSetFactory.CreateSequential(startFrameIndex: 0, frameCount: 1, speedMultiplicator: 1) },
                { AnimationSid.Skill3, AnimationFrameSetFactory.CreateSequential(startFrameIndex: 0, frameCount: 1, speedMultiplicator: 1) },
                { AnimationSid.Skill4, AnimationFrameSetFactory.CreateSequential(startFrameIndex: 0, frameCount: 1, speedMultiplicator: 1) },
                { AnimationSid.Ult, AnimationFrameSetFactory.CreateSequential(startFrameIndex: 0, frameCount: 1, speedMultiplicator: 1) },
                { AnimationSid.Wound, AnimationFrameSetFactory.CreateSequential(startFrameIndex: 24, frameCount: 8, speedMultiplicator: 8) },
                { AnimationSid.Death, AnimationFrameSetFactory.CreateSequential(startFrameIndex: 32, frameCount: 8, speedMultiplicator: 8) }
            };
        }
    }
}