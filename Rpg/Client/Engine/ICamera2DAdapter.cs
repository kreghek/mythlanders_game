using Microsoft.Xna.Framework;

namespace Client.Engine;

internal interface ICamera2DAdapter
{
    /// <summary>
    /// Center of the camera view.
    /// </summary>
    Vector2 ViewCenter { get; }

    /// <summary>
    /// Camera position.
    /// </summary>
    Vector2 Position { get; set; }

    /// <summary>
    /// Camera zoom.
    /// </summary>
    float Zoom { get; set; }

    /// <summary>
    /// Zoom in camera.
    /// </summary>
    void ZoomIn(float deltaZoom, Vector2 zoomCenter);

    /// <summary>
    /// Zoom out camera.
    /// </summary>
    void ZoomOut(float deltaZoom, Vector2 zoomCenter);

    /// <summary>
    /// Returns transformation matrix of game objects. 
    /// </summary>
    Matrix GetViewTransformationMatrix();

    /// <summary>
    /// Convert screen coordinates to coordinates in the world.
    /// </summary>
    /// <param name="screenPosition">Coordinates of screen. Example, mouse coordinates.</param>
    Vector2 ConvertScreenToWorldCoordinates(Vector2 screenPosition);
}