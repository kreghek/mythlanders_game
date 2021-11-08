using Microsoft.Xna.Framework;

namespace Rpg.Client.Engine
{
    internal class Camera2D
    {
        private Matrix _camRotationMatrix = Matrix.Identity;
        private Matrix _camScaleMatrix = Matrix.Identity;
        private Vector3 _camScaleVector = Vector3.Zero;
        private Matrix _camTranslationMatrix = Matrix.Identity;
        private Vector3 _camTranslationVector = Vector3.Zero;
        private bool _isViewTransformationDirty = true;
        private Vector2 _position;
        private Matrix _resTranslationMatrix = Matrix.Identity;
        private Vector3 _resTranslationVector = Vector3.Zero;
        private float _rotation;
        private Matrix _transform = Matrix.Identity;
        private float _zoom;

        protected ResolutionIndependentRenderer ResolutionIndependentRenderer;

        public Camera2D(ResolutionIndependentRenderer resolutionIndependence)
        {
            ResolutionIndependentRenderer = resolutionIndependence;

            _zoom = 0.1f;
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

        public Matrix GetViewTransformationMatrix(Vector2? additionalPosition = null)
        {
            if (_isViewTransformationDirty)
            {
                _camTranslationVector.X = -_position.X + additionalPosition.GetValueOrDefault().X;
                _camTranslationVector.Y = -_position.Y + additionalPosition.GetValueOrDefault().Y;

                Matrix.CreateTranslation(ref _camTranslationVector, out _camTranslationMatrix);

                Matrix.CreateRotationZ(_rotation, out _camRotationMatrix);

                _camScaleVector.X = _zoom;
                _camScaleVector.Y = _zoom;
                _camScaleVector.Z = 1;

                Matrix.CreateScale(ref _camScaleVector, out _camScaleMatrix);

                _resTranslationVector.X = ResolutionIndependentRenderer.VirtualWidth * 0.5f;
                _resTranslationVector.Y = ResolutionIndependentRenderer.VirtualHeight * 0.5f;
                _resTranslationVector.Z = 0;

                Matrix.CreateTranslation(ref _resTranslationVector, out _resTranslationMatrix);

                _transform = _camTranslationMatrix *
                             _camRotationMatrix *
                             _camScaleMatrix *
                             _resTranslationMatrix *
                             ResolutionIndependentRenderer.GetTransformationMatrix();

                _isViewTransformationDirty = false;
            }

            return _transform;
        }

        public void Move(Vector2 amount)
        {
            Position += amount;
        }

        public void RecalculateTransformationMatrices()
        {
            _isViewTransformationDirty = true;
        }

        public void SetPosition(Vector2 position)
        {
            Position = position;
        }
    }
}