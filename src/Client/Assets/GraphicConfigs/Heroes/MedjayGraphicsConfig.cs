using System.Collections.Generic;

using Client.Core;

using GameClient.Engine.Animations;

using Microsoft.Xna.Framework;

namespace Client.Assets.GraphicConfigs.Heroes;

internal sealed class MedjayGraphicsConfig : HeroGraphicConfig
{
    public MedjayGraphicsConfig() : base("Medjay")
    {
        LaunchPoint = new Vector2(-58, 48);
    }

    public override IDictionary<PredefinedAnimationSid, IAnimationFrameSet> GetPredefinedAnimations()
    {
        return new Dictionary<PredefinedAnimationSid, IAnimationFrameSet>
        {
            {
                PredefinedAnimationSid.Idle,
                AnimationFrameSetFactory.CreateIdle(startFrameIndex: 0, frameCount: 1, fps: 8)
            },
            {
                PredefinedAnimationSid.MoveForward,
                AnimationFrameSetFactory.CreateSequential(startFrameIndex: 0, frameWidth: 128, frameCount: 1, fps: 8)
            },
            {
                PredefinedAnimationSid.MoveBackward,
                AnimationFrameSetFactory.CreateSequential(startFrameIndex: 0, frameWidth: 128, frameCount: 1, fps: 8)
            },
            {
                PredefinedAnimationSid.Wound,
                AnimationFrameSetFactory.CreateSequential(startFrameIndex: 0, frameWidth: 128, frameCount: 1, fps: 8)
            },
            {
                PredefinedAnimationSid.Defense,
                AnimationFrameSetFactory.CreateSequential(startFrameIndex: 0, frameWidth: 128, frameCount: 1, fps: 8)
            },
            {
                PredefinedAnimationSid.Death,
                AnimationFrameSetFactory.CreateSequential(startFrameIndex: 0, frameWidth: 128, frameCount: 1, fps: 8)
            }
        };
    }
}