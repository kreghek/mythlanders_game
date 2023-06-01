using System.Collections.Generic;

using Client.Assets.GraphicConfigs.Heroes;

using Microsoft.Xna.Framework;

using Rpg.Client.Core;

namespace Rpg.Client.Assets.GraphicConfigs.Heroes
{
    internal sealed class MaosinGraphicsConfig : HeroGraphicConfig
    {
        public MaosinGraphicsConfig(UnitName name) : base(name)
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
                    PredefinedAnimationSid.Skill1,
                    AnimationFrameSetFactory.CreateSequential(startFrameIndex: 8, frameCount: 16, fps: 8)
                },
                {
                    PredefinedAnimationSid.Skill2,
                    AnimationFrameSetFactory.CreateSequential(startFrameIndex: 24, frameCount: 8, fps: 8)
                },
                {
                    PredefinedAnimationSid.Skill3,
                    AnimationFrameSetFactory.CreateSequential(startFrameIndex: 32, frameCount: 8, fps: 8)
                },
                {
                    PredefinedAnimationSid.Skill4,
                    AnimationFrameSetFactory.CreateSequential(startFrameIndex: 32, frameCount: 8, fps: 8)
                },
                {
                    PredefinedAnimationSid.Wound,
                    AnimationFrameSetFactory.CreateSequential(startFrameIndex: 48, frameCount: 8, fps: 8)
                },
                {
                    PredefinedAnimationSid.Death,
                    AnimationFrameSetFactory.CreateSequential(startFrameIndex: 56, frameCount: 8, fps: 8)
                }
            };
        }
    }
}