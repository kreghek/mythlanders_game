using System.Collections.Generic;

using Rpg.Client.Core;

namespace Rpg.Client.Assets.GraphicConfigs
{
    internal sealed class WispMonsterGraphicsConfig : UnitGraphicsConfigBase
    {
        public WispMonsterGraphicsConfig()
        {
            PredefinedAnimations = new Dictionary<AnimationSid, IAnimationFrameSet>
            {
                { AnimationSid.Idle, AnimationFrameSetFactory.CreateSequential(startFrameIndex: 0, frameCount: 8, speedMultiplicator: 12, isIdle: true, isLoop: true) },
                { AnimationSid.MoveForward, AnimationFrameSetFactory.CreateSequential(startFrameIndex: 0, frameCount: 1, speedMultiplicator: 1) },
                { AnimationSid.MoveBackward, AnimationFrameSetFactory.CreateSequential(startFrameIndex: 0, frameCount: 1, speedMultiplicator: 1) },
                { AnimationSid.Skill1, AnimationFrameSetFactory.CreateSequential(startFrameIndex: 8, frameCount: 8, speedMultiplicator: 12) },
                { AnimationSid.Skill2, AnimationFrameSetFactory.CreateSequential(startFrameIndex: 0, frameCount: 1, speedMultiplicator: 1) },
                { AnimationSid.Skill3, AnimationFrameSetFactory.CreateSequential(startFrameIndex: 0, frameCount: 1, speedMultiplicator: 1) },
                { AnimationSid.Wound, AnimationFrameSetFactory.CreateSequential(startFrameIndex: 16, frameCount: 2, speedMultiplicator: 8) },
                { AnimationSid.Death, AnimationFrameSetFactory.CreateSequential(startFrameIndex: 24, frameCount: 8, speedMultiplicator: 8) }
            };
        }
    }
}