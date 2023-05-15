using Microsoft.Xna.Framework;

namespace Rpg.Client.Engine
{
    internal class Camera2D
    {
        private bool _isViewTransformationDirty = true;
        private Vector2 _position;
        private float _rotation;
        private Matrix _transform = Matrix.Identity;
        private float _zoom;

        protected ResolutionIndependentRenderer ResolutionIndependentRenderer;

        public Camera2D(ResolutionIndependentRenderer resolutionIndependence)
        {
            ResolutionIndependentRenderer = resolutionIndependence;

            _zoom = 1f;
            _rotation = 0.0f;
            _position = Vector2.Zero;
        }

        public Vector2 Position
        {
            get => _position;
            set
            {
                _position = value;
                _isViewTransformationDirty = true;
            }
        }

        public float Rotation
        {
            get => _rotation;
            set
            {
                _rotation = value;
                _isViewTransformationDirty = true;
            }
        }

        public float Zoom
        {
            get => _zoom;
            set
            {
                _zoom = value;
                if (_zoom < 0.1f)
                {
                    _zoom = 0.1f;
                }

                _isViewTransformationDirty = true;
            }
        }

        /// <summary>
        /// // WARNING. This method changes inner state.
        /// </summary>
        /// <returns></returns>
        public Matrix GetViewTransformationMatrix()
        {
            if (_isViewTransformationDirty)
            {
                var virtualCenterWidth = ResolutionIndependentRenderer.VirtualWidth * 0.5f;
                var virtualCenterHeight = ResolutionIndependentRenderer.VirtualHeight * 0.5f;
                var origin = new Vector2(virtualCenterWidth * 1/Zoom - virtualCenterWidth,
                    virtualCenterHeight * 1/Zoom - virtualCenterHeight);

                _transform = Matrix.CreateTranslation(new Vector3(-_position, 0)) *
                             //Matrix.CreateTranslation(new Vector3(-origin, 0)) *
                             //Matrix.CreateTranslation(new Vector3(-origin, 0.0f))*
                             Matrix.CreateRotationZ(_rotation) *
                             Matrix.CreateScale(_zoom, _zoom, 1) *
                             Matrix.CreateTranslation(new Vector3(virtualCenterWidth, virtualCenterHeight, 0.0f)) *
                             ResolutionIndependentRenderer.GetTransformationMatrix();

                _isViewTransformationDirty = false;
            }

            return _transform;
        }

        public void RecalculateTransformationMatrices()
        {
            _isViewTransformationDirty = true;
        }
    }
}