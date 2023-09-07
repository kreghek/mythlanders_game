using Microsoft.Xna.Framework;

using MonoGame.Extended.ViewportAdapters;

namespace Client.Engine;

internal interface IResolutionIndependentRenderer : IScreenProjection
{
    BoxingViewportAdapter ViewportAdapter { get; }
    Rectangle VirtualBounds { get; }
    void BeginDraw();

    void Initialize();
}