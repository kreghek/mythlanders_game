using System.Collections.Generic;

using Client.Core;

namespace Client.Assets.GraphicConfigs.Heroes;

internal sealed class RobberGraphicsConfig : HeroGraphicConfig
{
    public RobberGraphicsConfig(UnitName name) : base(name)
    {
    }

    public override IDictionary<PredefinedAnimationSid, IAnimationFrameSet> GetPredefinedAnimations()
    {
        return new Dictionary<PredefinedAnimationSid, IAnimationFrameSet>
        {
            { PredefinedAnimationSid.Idle, AnimationFrameSetFactory.CreateIdle() },
            { PredefinedAnimationSid.MoveBackward, AnimationFrameSetFactory.CreateIdle() },
            { PredefinedAnimationSid.MoveForward, AnimationFrameSetFactory.CreateIdle() },
            {
                PredefinedAnimationSid.Wound,
                AnimationFrameSetFactory.CreateSequential(startFrameIndex: 32, frameCount: 8, fps: 8)
            },
            {
                PredefinedAnimationSid.Death,
                AnimationFrameSetFactory.CreateSequential(startFrameIndex: 40, frameCount: 8, fps: 8)
            },
            {
                PredefinedAnimationSid.DefenseStance,
                AnimationFrameSetFactory.CreateSequential(startFrameIndex: 16, frameCount: 1, fps: 1)
            }
        };
    }
}