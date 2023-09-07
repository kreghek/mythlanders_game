using System.Collections.Generic;

using Client.Core;

using Microsoft.Xna.Framework;

namespace Client.Assets.GraphicConfigs.Monsters;

internal sealed class VolkolakWarriorGraphicsConfig : SlavicMonsterGraphicConfig
{
    public VolkolakWarriorGraphicsConfig(UnitName unit) : base(unit)
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