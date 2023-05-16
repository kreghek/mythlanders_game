using Client.Engine;

using Microsoft.Xna.Framework;

namespace Client.GameScreens.Combat;

/// <summary>
/// The task to do camera operator.
/// </summary>
internal interface ICameraOperatorTask
{
    /// <summary>
    /// Is task complete.
    /// </summary>
    bool IsComplete { get; }
    
    /// <summary>
    /// Operate the specified camera.
    /// </summary>
    void DoWork(GameTime gameTime, ICamera2DAdapter camera);
}