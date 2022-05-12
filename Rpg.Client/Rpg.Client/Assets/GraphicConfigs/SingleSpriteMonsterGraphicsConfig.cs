using System.Collections.Generic;

using Rpg.Client.Core;

namespace Rpg.Client.Assets.GraphicConfigs
{
    internal sealed class SingleSpriteMonsterGraphicsConfig : UnitGraphicsConfigBase
    {
        public SingleSpriteMonsterGraphicsConfig()
        {
            PredefinedAnimations = new Dictionary<AnimationSid, IAnimationFrameSet>
            {
                { AnimationSid.Idle, AnimationFrameSetFactory.CreateIdle(startFrameIndex: 0, frameCount: 1, speedMultiplicator: 1) },
                { AnimationSid.Defense, AnimationFrameSetFactory.CreateSequential(startFrameIndex: 0, frameCount: 1, speedMultiplicator: 1) },

                { AnimationSid.MoveForward, AnimationFrameSetFactory.CreateSequential(startFrameIndex: 0, frameCount: 1, speedMultiplicator: 1) },
                { AnimationSid.MoveBackward, AnimationFrameSetFactory.CreateSequential(startFrameIndex: 0, frameCount: 1, speedMultiplicator: 1) },

                { AnimationSid.Skill1, AnimationFrameSetFactory.CreateSequential(startFrameIndex: 0, frameCount: 1, speedMultiplicator: 1) },
                { AnimationSid.Skill2, AnimationFrameSetFactory.CreateSequential(startFrameIndex: 0, frameCount: 1, speedMultiplicator: 1) },
                { AnimationSid.Skill3, AnimationFrameSetFactory.CreateSequential(startFrameIndex: 0, frameCount: 1, speedMultiplicator: 1) },
                { AnimationSid.Skill4, AnimationFrameSetFactory.CreateSequential(startFrameIndex: 0, frameCount: 1, speedMultiplicator: 1) },

                { AnimationSid.ShapeShift, AnimationFrameSetFactory.CreateSequential(startFrameIndex: 0, frameCount: 1, speedMultiplicator: 1) },
                { AnimationSid.Wound, AnimationFrameSetFactory.CreateSequential(startFrameIndex: 0, frameCount: 1, speedMultiplicator: 1) },
                { AnimationSid.Death, AnimationFrameSetFactory.CreateSequential(startFrameIndex: 1000, frameCount: 1, speedMultiplicator: 1) }
            };
        }
    }
}