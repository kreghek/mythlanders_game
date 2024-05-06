using System.Collections.Generic;

using Client.Core;

using GameClient.Engine.Animations;

using Microsoft.Xna.Framework;

namespace Client.Assets.GraphicConfigs.Heroes;

internal sealed class PartisanGraphicsConfig : HeroGraphicConfig
{
    public PartisanGraphicsConfig() : base("Partisan")
    {
        LaunchPoint = new Vector2(-58, 48);
    }

    public override IDictionary<PredefinedAnimationSid, IAnimationFrameSet> GetPredefinedAnimations()
    {
        return new Dictionary<PredefinedAnimationSid, IAnimationFrameSet>
        {
            {
                PredefinedAnimationSid.Idle,
                AnimationFrameSetFactory.CreateIdle(startFrameIndex: 0, frameCount: 8, fps: 8)
            },
            {
                PredefinedAnimationSid.MoveForward,
                AnimationFrameSetFactory.CreateSequential(startFrameIndex: 8 * 4, frameCount: 1, fps: 1)
            },
            {
                PredefinedAnimationSid.MoveBackward,
                AnimationFrameSetFactory.CreateSequential(startFrameIndex: 8 * 4, frameCount: 1, fps: 1)
            },
            {
                PredefinedAnimationSid.Wound,
                AnimationFrameSetFactory.CreateSequential(startFrameIndex: 8 * 1, frameCount: 1, fps: 1, isLoop: false)
            },
            {
                PredefinedAnimationSid.Defense,
                AnimationFrameSetFactory.CreateSequential(startFrameIndex: 8 * 1, frameCount: 1, fps: 1, isLoop: false)
            },
            {
                PredefinedAnimationSid.DefenseStance,
                AnimationFrameSetFactory.CreateSequential(startFrameIndex: 8 * 1, frameCount: 1, fps: 1, isLoop: true)
            },
            {
                PredefinedAnimationSid.Death,
                AnimationFrameSetFactory.CreateSequential(startFrameIndex: 8 * 1, frameCount: 1, fps: 1, isLoop: false)
            }
        };
    }
}