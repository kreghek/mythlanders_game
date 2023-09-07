using Microsoft.Xna.Framework;

namespace GameClient.Engine.RectControl;

/// <summary>
/// Base class of rect control to manipulate inner rects into the parent using some algorithms.
/// </summary>
public abstract class RectControlBase
{
    /// <summary>
    /// Gets calculated inner rects.
    /// </summary>
    /// <returns> Array of inner rect ordered by a algorithm. </returns>
    // ReSharper disable once UnusedMemberInSuper.Global
    public abstract IReadOnlyList<Rectangle> GetRects();
}