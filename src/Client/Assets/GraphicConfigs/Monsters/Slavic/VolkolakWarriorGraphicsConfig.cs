using System.Collections.Generic;
using System.IO;

using Client.Assets.CombatMovements;
using Client.Core;

using Microsoft.Xna.Framework;

namespace Client.Assets.GraphicConfigs.Monsters.Slavic;

internal sealed class VolkolakWarriorGraphicsConfig : SlavicMonsterGraphicConfig
{
    public VolkolakWarriorGraphicsConfig(UnitName unit) : base(unit)
    {
        StatsPanelOrigin = new Vector2(-16, 64 + 8);
        ShadowOffset = new Vector2(-16, -16);
    }

    public override string ThumbnailPath => Path.Combine(CommonConstants.PathToCharacterSprites, "Monsters",
        CultureSid.ToString(), "Volkolak", "Thumbnail");

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