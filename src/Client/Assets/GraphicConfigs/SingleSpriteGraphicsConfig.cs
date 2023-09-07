using System.Collections.Generic;

using Client.Core;

namespace Client.Assets.GraphicConfigs;

internal sealed class SingleSpriteGraphicsConfig : UnitGraphicsConfigBase
{
    public SingleSpriteGraphicsConfig(string thumbnailPath)
    {
        ThumbnailPath = thumbnailPath;
    }

    public override string ThumbnailPath { get; }

    public override IDictionary<PredefinedAnimationSid, IAnimationFrameSet> GetPredefinedAnimations()
    {
        return new Dictionary<PredefinedAnimationSid, IAnimationFrameSet>
        {
            {
                PredefinedAnimationSid.Idle,
                AnimationFrameSetFactory.CreateIdle(startFrameIndex: 0, frameCount: 1, fps: 1)
            },
            {
                PredefinedAnimationSid.DefenseStance,
                AnimationFrameSetFactory.CreateSequential(startFrameIndex: 0, frameCount: 1, fps: 1)
            },

            {
                PredefinedAnimationSid.MoveForward,
                AnimationFrameSetFactory.CreateSequential(startFrameIndex: 0, frameCount: 1, fps: 1)
            },
            {
                PredefinedAnimationSid.MoveBackward,
                AnimationFrameSetFactory.CreateSequential(startFrameIndex: 0, frameCount: 1, fps: 1)
            },
            {
                PredefinedAnimationSid.Wound,
                AnimationFrameSetFactory.CreateSequential(startFrameIndex: 0, frameCount: 1, fps: 1)
            },
            {
                PredefinedAnimationSid.Death,
                AnimationFrameSetFactory.CreateSequential(startFrameIndex: 1000, frameCount: 1, fps: 1)
            }
        };
    }
}