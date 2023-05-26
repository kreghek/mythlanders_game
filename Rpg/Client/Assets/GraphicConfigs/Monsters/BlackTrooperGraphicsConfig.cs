using System.Collections.Generic;

using Client.Assets.GraphicConfigs.Monsters;

using Rpg.Client.Core;

namespace Rpg.Client.Assets.GraphicConfigs.Monsters
{
    internal sealed class BlackTrooperGraphicsConfig : BlackMonsterGraphicConfig
    {
        public override IDictionary<PredefinedAnimationSid, IAnimationFrameSet> GetPredefinedAnimations()
        {
            return new Dictionary<PredefinedAnimationSid, IAnimationFrameSet>
            {
                { PredefinedAnimationSid.Idle, AnimationFrameSetFactory.CreateIdle(fps: 6) },
                {
                    PredefinedAnimationSid.MoveForward,
                    AnimationFrameSetFactory.CreateEmpty()
                },
                {
                    PredefinedAnimationSid.MoveBackward,
                    AnimationFrameSetFactory.CreateEmpty()
                },
                {
                    PredefinedAnimationSid.Skill1,
                    AnimationFrameSetFactory.CreateSequential(startFrameIndex: 8, frameCount: 8, fps: 8)
                },
                {
                    PredefinedAnimationSid.Skill2,
                    AnimationFrameSetFactory.CreateEmpty()
                },
                {
                    PredefinedAnimationSid.Skill3,
                    AnimationFrameSetFactory.CreateEmpty()
                },
                {
                    PredefinedAnimationSid.Skill4,
                    AnimationFrameSetFactory.CreateEmpty()
                },
                {
                    PredefinedAnimationSid.Ult,
                    AnimationFrameSetFactory.CreateEmpty()
                },
                {
                    PredefinedAnimationSid.Wound,
                    AnimationFrameSetFactory.CreateSequential(startFrameIndex: 16, frameCount: 8, fps: 8)
                },
                {
                    PredefinedAnimationSid.Death,
                    AnimationFrameSetFactory.CreateSequential(startFrameIndex: 24, frameCount: 8, fps: 8)
                }
            };
        }
    }
}