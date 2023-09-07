using System.Collections.Generic;

using Client.Core;

namespace Client.Assets.GraphicConfigs.Heroes;

internal sealed class GenericHeroGraphicsConfig : HeroGraphicConfig
{
    public GenericHeroGraphicsConfig(UnitName name) : base(name)
    {
    }

    public override IDictionary<PredefinedAnimationSid, IAnimationFrameSet> GetPredefinedAnimations()
    {
        return new Dictionary<PredefinedAnimationSid, IAnimationFrameSet>
        {
            { PredefinedAnimationSid.Idle, AnimationFrameSetFactory.CreateIdle() },
            {
                PredefinedAnimationSid.Wound,
                AnimationFrameSetFactory.CreateSequential(startFrameIndex: 32, frameCount: 8, fps: 8)
            },
            {
                PredefinedAnimationSid.Death,
                AnimationFrameSetFactory.CreateSequential(startFrameIndex: 40, frameCount: 8, fps: 8)
            }
        };
    }
}