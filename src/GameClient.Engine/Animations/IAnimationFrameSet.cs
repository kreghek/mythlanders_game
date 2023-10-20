using Microsoft.Xna.Framework;

namespace GameClient.Engine.Animations;

/// <summary>
/// Animation of the sprite by sprite sheet.
/// </summary>
public interface IAnimationFrameSet
{
    //TODO Remove
    /// <summary>
    /// Marker the animation is idle of combatant.
    /// Used only for showing of the selection marker.
    /// </summary>
    bool IsIdle { get; }

    /// <summary>
    /// Current source frame from sprite sheet.
    /// </summary>
    /// <returns></returns>
    Rectangle GetFrameRect();

    /// <summary>
    /// Reset animation to start state.
    /// </summary>
    void Reset();

    /// <summary>
    /// Update animation state.
    /// </summary>
    /// <param name="gameTime"> Game time to calculate animation state. </param>
    void Update(GameTime gameTime);

    /// <summary>
    /// Raise the animation ends.
    /// </summary>
    event EventHandler? End;

    /// <summary>
    /// Raise every frame of animation.
    /// </summary>
    /// <remarks>
    /// Do not support correctly by all of implementations.
    /// </remarks>
    event EventHandler<AnimationFrameEventArgs> KeyFrame;
}