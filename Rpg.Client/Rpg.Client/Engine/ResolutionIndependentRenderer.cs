using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Rpg.Client.Engine
{
    internal class ResolutionIndependentRenderer
    {
        private static Matrix _scaleMatrix;
        private readonly Game _game;
        private bool _dirtyMatrix = true;
        private float _ratioX;
        private float _ratioY;
        private Viewport _viewport;
        private Vector2 _virtualMousePosition;

        private readonly Color _backgroundColor = Color.Black;

        public int ScreenHeight;

        public int ScreenWidth;

        public int VirtualHeight;

        public int VirtualWidth;

        public ResolutionIndependentRenderer(Game game)
        {
            _game = game;
            VirtualWidth = 1366;
            VirtualHeight = 768;

            ScreenWidth = 1024;
            ScreenHeight = 768;
        }

        public Rectangle VirtualBounds => new Rectangle(0, 0, VirtualWidth, VirtualHeight);

        public void BeginDraw()
        {
            // Start by reseting viewport to (0,0,1,1)
            SetupFullViewport();
            // Clear to Black
            _game.GraphicsDevice.Clear(_backgroundColor);
            // Calculate Proper Viewport according to Aspect Ratio
            SetupVirtualScreenViewport();
            // and clear that
            // This way we are gonna have black bars if aspect ratio requires it and
            // the clear color on the rest
        }

        public Matrix GetTransformationMatrix()
        {
            if (_dirtyMatrix)
            {
                RecreateScaleMatrix();
            }

            return _scaleMatrix;
        }

        public void Initialize()
        {
            SetupVirtualScreenViewport();

            _ratioX = (float)_viewport.Width / VirtualWidth;
            _ratioY = (float)_viewport.Height / VirtualHeight;

            _dirtyMatrix = true;
        }

        public Vector2 ScaleMouseToScreenCoordinates(Vector2 screenPosition)
        {
            var realX = screenPosition.X - _viewport.X;
            var realY = screenPosition.Y - _viewport.Y;

            _virtualMousePosition.X = realX / _ratioX;
            _virtualMousePosition.Y = realY / _ratioY;

            return _virtualMousePosition;
        }

        private void SetupFullViewport()
        {
            var vp = new Viewport();
            vp.X = vp.Y = 0;
            vp.Width = ScreenWidth;
            vp.Height = ScreenHeight;
            _game.GraphicsDevice.Viewport = vp;
            _dirtyMatrix = true;
        }

        private void SetupVirtualScreenViewport()
        {
            var targetAspectRatio = VirtualWidth / (float)VirtualHeight;
            // figure out the largest area that fits in this resolution at the desired aspect ratio
            var width = ScreenWidth;
            var height = (int)(width / targetAspectRatio + .5f);

            if (height > ScreenHeight)
            {
                height = ScreenHeight;
                // PillarBox
                width = (int)(height * targetAspectRatio + .5f);
            }

            // set up the new viewport centered in the back-buffer
            _viewport = new Viewport
            {
                X = (ScreenWidth / 2) - (width / 2),
                Y = (ScreenHeight / 2) - (height / 2),
                Width = width,
                Height = height
            };

            _game.GraphicsDevice.Viewport = _viewport;
        }

        private void RecreateScaleMatrix()
        {
            Matrix.CreateScale((float)ScreenWidth / VirtualWidth, (float)ScreenWidth / VirtualWidth, 1f,
                out _scaleMatrix);
            _dirtyMatrix = false;
        }
    }
}