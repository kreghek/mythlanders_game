using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Rpg.Client.Core;

namespace Rpg.Client.Assets.GraphicConfigs.Monsters
{
    internal sealed class VolkolakWarriorGraphicsConfig : UnitGraphicsConfigBase
    {
        public VolkolakWarriorGraphicsConfig()
        {
            StatsPanelOrigin = new Vector2(-16, 64 + 8);
            ShadowOrigin = new Vector2(-16, -16);
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
                    PredefinedAnimationSid.Wound,
                    AnimationFrameSetFactory.CreateSequential(startFrameIndex: 16, frameCount: 8, fps: 8)
                },
                {
                    PredefinedAnimationSid.Death,
                    AnimationFrameSetFactory.CreateSequential(startFrameIndex: 24, frameCount: 8, fps: 8)
                },
                {
                    PredefinedAnimationSid.ShapeShift,
                    AnimationFrameSetFactory.CreateSequential(startFrameIndex: 24, frameCount: 16, fps: 8)
                }
            };
        }
    }
}