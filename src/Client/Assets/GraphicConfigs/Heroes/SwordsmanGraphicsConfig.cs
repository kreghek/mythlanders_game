using System.Collections.Generic;

using Client.Core;

using GameClient.Engine.Animations;

namespace Client.Assets.GraphicConfigs.Heroes;

internal sealed class SwordsmanGraphicsConfig : HeroGraphicConfig
{
    public SwordsmanGraphicsConfig(UnitName name) : base(name)
    {
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
                PredefinedAnimationSid.DefenseStance,
                AnimationFrameSetFactory.CreateSequential(startFrameIndex: 16, frameCount: 1, fps: 0)
            },
            {
                PredefinedAnimationSid.MoveForward,
                AnimationFrameSetFactory.CreateSequential(startFrameIndex: 32, frameCount: 8, fps: 6)
            },
            {
                PredefinedAnimationSid.MoveBackward,
                AnimationFrameSetFactory.CreateSequential(startFrameIndex: 32, frameCount: 8, fps: 6)
            },
            {
                PredefinedAnimationSid.Wound,
                AnimationFrameSetFactory.CreateSequential(startFrameIndex: 40, frameCount: 8, fps: 8, isLoop: false)
            },
            {
                PredefinedAnimationSid.Defense,
                AnimationFrameSetFactory.CreateSequential(startFrameIndex: 16, frameCount: 8, fps: 16, isLoop: false)
            },
            {
                PredefinedAnimationSid.Death,
                AnimationFrameSetFactory.CreateSequential(startFrameIndex: 48, frameCount: 8, fps: 8, isLoop: false)
            }
        };
    }
}