using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Engine;

namespace Rpg.Client.GameScreens.Combat.GameObjects
{
    internal sealed class CorpseGameObject : EwarDrawableComponentBase
    {
        private readonly Camera2D _camera;
        private readonly GameObjectContentStorage _gameObjectContentStorage;
        private readonly UnitGraphics _graphics;
        private readonly ScreenShaker _screenShaker;
        private bool _startToWound;
        private double _counter;

        public CorpseGameObject(UnitGraphics graphics, Camera2D camera, ScreenShaker screenShaker,
            GameObjectContentStorage gameObjectContentStorage)
        {
            _graphics = graphics;
            _camera = camera;
            _screenShaker = screenShaker;
            _gameObjectContentStorage = gameObjectContentStorage;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            _graphics.Update(gameTime);

            _counter += gameTime.ElapsedGameTime.TotalSeconds;

            if (_counter > 0.05)
            {
                _graphics.IsDamaged = false;
            }

            if (_counter > 1 && !_startToDeath)
            {
                _startToDeath = true;
                _graphics.IsDamaged = false;
                _graphics.PlayAnimation("Death");
            }

            if (!_startToWound)
            {
                _graphics.PlayAnimation("Wound");
                _startToWound = true;
            }
        }

        private bool _startToDeath;

        protected override void DoDraw(SpriteBatch spriteBatch, float zindex)
        {
            base.DoDraw(spriteBatch, zindex);

            _graphics.ShowActiveMarker = false;

            if (_graphics.IsDamaged)
            {
                var allWhite = _gameObjectContentStorage.GetAllWhiteEffect();
                spriteBatch.End();

                var shakeVector = _screenShaker.GetOffset().GetValueOrDefault(Vector2.Zero);
                var shakeVector3d = new Vector3(shakeVector, 0);

                var worldTransformationMatrix = _camera.GetViewTransformationMatrix();
                worldTransformationMatrix.Decompose(out var scaleVector, out _, out var translationVector);

                var matrix = Matrix.CreateTranslation(translationVector + shakeVector3d)
                             * Matrix.CreateScale(scaleVector);

                spriteBatch.Begin(sortMode: SpriteSortMode.Deferred,
                    blendState: BlendState.AlphaBlend,
                    samplerState: SamplerState.PointClamp,
                    depthStencilState: DepthStencilState.None,
                    rasterizerState: RasterizerState.CullNone,
                    transformMatrix: matrix,
                    effect: allWhite);
            }
            else
            {
                spriteBatch.End();

                var shakeVector = _screenShaker.GetOffset().GetValueOrDefault(Vector2.Zero);
                var shakeVector3d = new Vector3(shakeVector, 0);

                var worldTransformationMatrix = _camera.GetViewTransformationMatrix();
                worldTransformationMatrix.Decompose(out var scaleVector, out _, out var translationVector);

                var matrix = Matrix.CreateTranslation(translationVector + shakeVector3d)
                             * Matrix.CreateScale(scaleVector);

                spriteBatch.Begin(sortMode: SpriteSortMode.Deferred,
                    blendState: BlendState.AlphaBlend,
                    samplerState: SamplerState.PointClamp,
                    depthStencilState: DepthStencilState.None,
                    rasterizerState: RasterizerState.CullNone,
                    transformMatrix: matrix);
            }

            _graphics.Draw(spriteBatch);

            spriteBatch.End();

            spriteBatch.Begin(sortMode: SpriteSortMode.Deferred,
                blendState: BlendState.AlphaBlend,
                samplerState: SamplerState.PointClamp,
                depthStencilState: DepthStencilState.None,
                rasterizerState: RasterizerState.CullNone,
                transformMatrix: _camera.GetViewTransformationMatrix());
        }

        internal float GetZIndex()
        {
            return _graphics.Root.Position.Y;
        }
    }
}