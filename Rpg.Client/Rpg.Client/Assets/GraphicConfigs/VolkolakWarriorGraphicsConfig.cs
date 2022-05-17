using System.Collections.Generic;

using Rpg.Client.Core;

namespace Rpg.Client.Assets.GraphicConfigs
{
    internal sealed class VolkolakWarriorGraphicsConfig : UnitGraphicsConfigBase
    {
        public VolkolakWarriorGraphicsConfig()
        {
            PredefinedAnimations = new Dictionary<PredefinedAnimationSid, IAnimationFrameSet>
            {
                { PredefinedAnimationSid.Idle, AnimationFrameSetFactory.CreateIdle() },
                { PredefinedAnimationSid.Skill1, AnimationFrameSetFactory.CreateSequential(startFrameIndex: 8, frameCount: 8, fps: 8) },
                { PredefinedAnimationSid.Wound, AnimationFrameSetFactory.CreateSequential(startFrameIndex: 16, frameCount: 8, fps: 8) },
                { PredefinedAnimationSid.Death, AnimationFrameSetFactory.CreateSequential(startFrameIndex: 24, frameCount: 8, fps: 8) },
                { PredefinedAnimationSid.ShapeShift, AnimationFrameSetFactory.CreateSequential(startFrameIndex: 24, frameCount: 16, fps: 8) }
            };
        }
    }
}