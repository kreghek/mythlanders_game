using System.Collections.Generic;

using Client.Assets.GraphicConfigs.Heroes;

using Rpg.Client.Core;

namespace Rpg.Client.Assets.GraphicConfigs.Heroes
{
    internal sealed class GuardsmanGraphicsConfig : HeroGraphicConfig
    {
        public GuardsmanGraphicsConfig(UnitName name) : base(name)
        {
        }

        public override IDictionary<PredefinedAnimationSid, IAnimationFrameSet> GetPredefinedAnimations()
        {
            return new Dictionary<PredefinedAnimationSid, IAnimationFrameSet>
            {
                { PredefinedAnimationSid.Idle, AnimationFrameSetFactory.CreateIdle() },
                {
                    PredefinedAnimationSid.Defense,
                    AnimationFrameSetFactory.CreateSequential(startFrameIndex: 39, frameCount: 1, fps: 0)
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
                    AnimationFrameSetFactory.CreateSequential(startFrameIndex: 32, frameCount: 8, fps: 8)
                },

                {
                    PredefinedAnimationSid.Ult,
                    AnimationFrameSetFactory.CreateSequential(startFrameIndex: 59, frameCount: 2, fps: 8)
                },

                {
                    PredefinedAnimationSid.Wound,
                    AnimationFrameSetFactory.CreateSequential(startFrameIndex: 40, frameCount: 8, fps: 8)
                },
                {
                    PredefinedAnimationSid.Death,
                    AnimationFrameSetFactory.CreateSequential(startFrameIndex: 48, frameCount: 8, fps: 8)
                }
            };
        }
    }
}