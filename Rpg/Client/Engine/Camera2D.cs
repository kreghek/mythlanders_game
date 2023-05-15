using System;

using Microsoft.Xna.Framework;

using MonoGame.Extended;

namespace Rpg.Client.Engine
{
    internal class Camera2D
    {
        protected ResolutionIndependentRenderer ResolutionIndependentRenderer;
        private readonly OrthographicCamera _innerCamera;
        public Vector2 Origin => _innerCamera.Origin;

        public Camera2D(ResolutionIndependentRenderer resolutionIndependence, MonoGame.Extended.ViewportAdapters.BoxingViewportAdapter viewportAdapter)
        {
            ResolutionIndependentRenderer = resolutionIndependence;

            _innerCamera = new OrthographicCamera(viewportAdapter);
        }

        public Vector2 Position
        {
            get => _innerCamera.Position;
            set
            {
                _innerCamera.Position = value;
            }
        }

        public void LookAt(Vector2 position)
        { 
            _innerCamera.Position = position;
        }

        public float Zoom
        {
            get => _innerCamera.Zoom;
            set
            {
                _innerCamera.Zoom = value;
            }
        }

        public void ZoomIn(float deltaZoom, Vector2 zoomCenter)
        {
            float pastZoom = Zoom;
            _innerCamera.ZoomIn(deltaZoom);
            Position += (zoomCenter - _innerCamera.Origin - Position) * ((Zoom - pastZoom) / Zoom);
        }

        public void ZoomOut(float deltaZoom)
        {
            _innerCamera.ZoomOut(deltaZoom);
        }

        /// <summary>
        /// // WARNING. This method changes inner state.
        /// </summary>
        /// <returns></returns>
        public Matrix GetViewTransformationMatrix()
        {
            return _innerCamera.GetViewMatrix();
        }

        internal Vector2 ScaleMouseToScreenCoordinates(Vector2 screenPosition)
        {
            return _innerCamera.ScreenToWorld(screenPosition);
        }
    }
}