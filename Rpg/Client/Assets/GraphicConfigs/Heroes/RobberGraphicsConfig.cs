using System.Collections.Generic;

using Rpg.Client.Core;

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
                PredefinedAnimationSid.Skill1,
                AnimationFrameSetFactory.CreateSequential(startFrameIndex: 8, frameCount: 8, fps: 8)
            },
            {
                PredefinedAnimationSid.Skill2,
                AnimationFrameSetFactory.CreateSequential(startFrameIndex: 16, frameCount: 8, fps: 8)
            },
            {
                PredefinedAnimationSid.Skill3,
                AnimationFrameSetFactory.CreateSequential(startFrameIndex: 24, frameCount: 8, fps: 8)
            },
            {
                PredefinedAnimationSid.Skill4,
                AnimationFrameSetFactory.CreateSequential(startFrameIndex: 8, frameCount: 8, fps: 8)
            },
            {
                PredefinedAnimationSid.Ult,
                AnimationFrameSetFactory.CreateSequential(startFrameIndex: 24, frameCount: 8, fps: 8)
            },
            {
                PredefinedAnimationSid.Wound,
                AnimationFrameSetFactory.CreateSequential(startFrameIndex: 32, frameCount: 8, fps: 8)
            },
            {
                PredefinedAnimationSid.Death,
                AnimationFrameSetFactory.CreateSequential(startFrameIndex: 40, frameCount: 8, fps: 8)
            },
            {
                PredefinedAnimationSid.Defense,
                AnimationFrameSetFactory.CreateSequential(startFrameIndex: 16, frameCount: 1, fps: 1)
            }
        };
    }
}