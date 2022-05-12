using System.Collections.Generic;

using Rpg.Client.Core;

namespace Rpg.Client.Assets.GraphicConfigs
{
    internal sealed class HawkGraphicsConfig : UnitGraphicsConfigBase
    {
        public HawkGraphicsConfig()
        {
            PredefinedAnimations = new Dictionary<AnimationSid, AnimationFrameSet>
            {
                { AnimationSid.Idle, AnimationFrameSetFactory.CreateSequential(startFrameIndex: 0, frameCount: 8, speedMultiplicator: 8, isIdle: true, isLoop: true) },
                { AnimationSid.Skill1, AnimationFrameSetFactory.CreateSequential(startFrameIndex: 8, frameCount: 8, speedMultiplicator: 8) },
                { AnimationSid.Skill2, AnimationFrameSetFactory.CreateSequential(startFrameIndex: 16, frameCount: 8, speedMultiplicator: 8) },
                { AnimationSid.Skill3, AnimationFrameSetFactory.CreateSequential(startFrameIndex: 24, frameCount: 8, speedMultiplicator: 8) },
                { AnimationSid.Skill4, AnimationFrameSetFactory.CreateSequential(startFrameIndex: 8, frameCount: 8, speedMultiplicator: 8) },
                { AnimationSid.Ult, AnimationFrameSetFactory.CreateSequential(startFrameIndex: 24, frameCount: 8, speedMultiplicator: 8) },
                { AnimationSid.Wound, AnimationFrameSetFactory.CreateSequential(startFrameIndex: 32, frameCount: 8, speedMultiplicator: 8) },
                { AnimationSid.Death, AnimationFrameSetFactory.CreateSequential(startFrameIndex: 40, frameCount: 8, speedMultiplicator: 8) },
                { AnimationSid.Defense, AnimationFrameSetFactory.CreateSequential(startFrameIndex: 16, frameCount: 1, speedMultiplicator: 1) }
            };
        }
    }
}