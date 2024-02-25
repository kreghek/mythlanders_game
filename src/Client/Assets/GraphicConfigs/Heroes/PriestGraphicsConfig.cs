using System.Collections.Generic;

using Client.Core;

using GameClient.Engine.Animations;

namespace Client.Assets.GraphicConfigs.Heroes;

internal sealed class PriestGraphicsConfig : HeroGraphicConfig
{
    public PriestGraphicsConfig() : base(UnitName.Priest.ToString())
    {
        Origin = new Microsoft.Xna.Framework.Vector2(64, 120);
    }

    public override IDictionary<PredefinedAnimationSid, IAnimationFrameSet> GetPredefinedAnimations()
    {
        return new Dictionary<PredefinedAnimationSid, IAnimationFrameSet>
        {
            { PredefinedAnimationSid.Idle, AnimationFrameSetFactory.CreateIdle(startFrameIndex: 0, frameCount: 1) },
            {
                PredefinedAnimationSid.DefenseStance,
                AnimationFrameSetFactory.CreateIdle(startFrameIndex: 0, frameCount: 1)
            },
            {
                PredefinedAnimationSid.MoveForward,
                AnimationFrameSetFactory.CreateIdle(startFrameIndex: 0, frameCount: 1)
            },
            {
                PredefinedAnimationSid.MoveBackward,
                AnimationFrameSetFactory.CreateIdle(startFrameIndex: 0, frameCount: 1)
            },
            {
                PredefinedAnimationSid.Wound,
                AnimationFrameSetFactory.CreateIdle(startFrameIndex : 0, frameCount : 1)
            },
            {
                PredefinedAnimationSid.Death,
                AnimationFrameSetFactory.CreateIdle(startFrameIndex : 0, frameCount : 1)
            }
        };
    }
}