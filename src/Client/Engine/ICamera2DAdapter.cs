using Microsoft.Xna.Framework;

namespace Client.Engine;

internal interface ICamera2DAdapter: IScreenProjection
{
    /// <summary>
    /// Camera position.
    /// </summary>
    Vector2 Position { get; set; }

    /// <summary>
    /// Camera zoom.
    /// </summary>
    float Zoom { get; set; }

    /// <summary>
    /// Returns transformation matrix of game objects.
    /// </summary>
    Matrix GetViewTransformationMatrix();

    /// <summary>
    /// Zoom in camera.
    /// </summary>
    void ZoomIn(float deltaZoom, Vector2 zoomCenter);

    /// <summary>
    /// Zoom out camera.
    /// </summary>
    void ZoomOut(float deltaZoom, Vector2 zoomCenter);
}