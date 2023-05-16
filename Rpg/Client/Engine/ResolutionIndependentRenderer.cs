using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoGame.Extended.ViewportAdapters;

namespace Rpg.Client.Engine
{
    internal class ResolutionIndependentRenderer
    {
        private readonly Camera2D _camera;

        public int VirtualHeight;

        public int VirtualWidth;

        public ResolutionIndependentRenderer(Camera2D camera, BoxingViewportAdapter viewportAdapter)
        {
            _camera = camera;
            ViewportAdapter = viewportAdapter;
            VirtualWidth = 1366;
            VirtualHeight = 768;
        }

        public Rectangle VirtualBounds => ViewportAdapter.BoundingRectangle;

        public BoxingViewportAdapter ViewportAdapter { get; }

        public void BeginDraw()
        {
        }
        

        public void Initialize()
        {
        }

        public Vector2 ScaleMouseToScreenCoordinates(Vector2 screenPosition)
        {
            return _camera.ScaleMouseToScreenCoordinates(screenPosition);
        }
    }
}