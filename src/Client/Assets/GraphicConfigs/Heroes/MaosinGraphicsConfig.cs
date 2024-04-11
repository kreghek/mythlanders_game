using System.Collections.Generic;

using Client.Core;

using GameClient.Engine.Animations;

using Microsoft.Xna.Framework;

namespace Client.Assets.GraphicConfigs.Heroes;

internal sealed class MaosinGraphicsConfig : HeroGraphicConfig
{
    public MaosinGraphicsConfig(string name) : base(name)
    {
        InteractionPoint = new Vector2(0, 64);
        StatsPanelOrigin = new Vector2(0, 64 + 16);
    }

    public override IDictionary<PredefinedAnimationSid, IAnimationFrameSet> GetPredefinedAnimations()
    {
        return new Dictionary<PredefinedAnimationSid, IAnimationFrameSet>
        {
            { PredefinedAnimationSid.Idle, AnimationFrameSetFactory.CreateIdle() },
            {
                PredefinedAnimationSid.MoveForward,
                AnimationFrameSetFactory.CreateSequential(startFrameIndex: 40, frameCount: 8, fps: 6)
            },
            {
                PredefinedAnimationSid.MoveBackward,
                AnimationFrameSetFactory.CreateSequential(startFrameIndex: 40, frameCount: 8, fps: 8)
            },
            {
                PredefinedAnimationSid.Wound,
                AnimationFrameSetFactory.CreateSequential(startFrameIndex: 48, frameCount: 8, fps: 8)
            },
            {
                PredefinedAnimationSid.Death,
                AnimationFrameSetFactory.CreateSequential(startFrameIndex: 56, frameCount: 8, fps: 8)
            },
            {
                PredefinedAnimationSid.DefenseStance,
                AnimationFrameSetFactory.CreateSequential(startFrameIndex: 16, frameCount: 1, fps: 1)
            },
            {
                PredefinedAnimationSid.Defense,
                AnimationFrameSetFactory.CreateSequential(startFrameIndex: 16, frameCount: 1, fps: 1)
            }
        };
    }
}