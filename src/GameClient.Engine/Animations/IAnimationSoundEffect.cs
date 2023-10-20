namespace GameClient.Engine.Animations;

/// <summary>
/// Wrapper to sound effect.
/// </summary>
/// <remarks>
/// Used in unit tests to avoid real sound effect creation.
/// </remarks>
public interface IAnimationSoundEffect
{
    /// <summary>
    /// Play sound effect.
    /// </summary>
    void Play();
}