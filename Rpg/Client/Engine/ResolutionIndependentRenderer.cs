using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoGame.Extended.ViewportAdapters;

namespace Rpg.Client.Engine
{
    internal class ResolutionIndependentRenderer
    {
        private static Matrix _scaleMatrix;

        private readonly Color _backgroundColor = Color.Black;
        private readonly Game _game;
        private readonly Camera2D _camera;
        private bool _dirtyMatrix = true;
        private float _ratioX;
        private float _ratioY;
        private Viewport _viewport;
        private Vector2 _virtualMousePosition;

        public int ScreenHeight;

        public int ScreenWidth;

        public int VirtualHeight;

        public int VirtualWidth;

        public ResolutionIndependentRenderer(Game game, Camera2D camera, MonoGame.Extended.ViewportAdapters.BoxingViewportAdapter viewportAdapter)
        {
            _game = game;
            _camera = camera;
            ViewportAdapter = viewportAdapter;
            VirtualWidth = 1366;
            VirtualHeight = 768;

            ScreenWidth = 1024;
            ScreenHeight = 768;
        }

        public Rectangle VirtualBounds => new (0, 0, VirtualWidth, VirtualHeight);

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