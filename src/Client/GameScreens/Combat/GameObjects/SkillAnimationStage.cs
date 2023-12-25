using System;

using Microsoft.Xna.Framework.Audio;

namespace Client.GameScreens.Combat.GameObjects;

internal sealed class SkillAnimationStage
{
    /// <summary>
    /// Duration of the item in seconds.
    /// </summary>
    public float Duration { get; set; }

    public SoundEffectInstance? HitSound { get; set; }

    public Action Interaction { get; set; }

    public float InteractTime { get; set; }
}