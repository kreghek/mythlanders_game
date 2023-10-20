using Microsoft.Xna.Framework.Audio;

namespace GameClient.Engine.Animations;

/// <summary>
/// Wrapper to play sound effect with specific way.
/// </summary>
public interface IAudioPlayer
{
    /// <summary>
    /// Play sound effect.
    /// </summary>
    /// <param name="soundEffect">Sound effect to play.</param>
    void PlayEffect(SoundEffect soundEffect);
}