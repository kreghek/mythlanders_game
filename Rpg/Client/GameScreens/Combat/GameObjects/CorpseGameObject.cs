using Client.Engine;
using Client.GameScreens.Combat.GameObjects;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Engine;

namespace Rpg.Client.GameScreens.Combat.GameObjects
{
    internal sealed class CorpseGameObject : EwarRenderableBase
    {
        private readonly ICamera2DAdapter _camera;
        private readonly GameObjectContentStorage _gameObjectContentStorage;
        private readonly UnitGraphics _graphics;
        private double _counter;

        private bool _startToDeath;
        private bool _startToWound;

        public CorpseGameObject(UnitGraphics graphics, ICamera2DAdapter camera,
            GameObjectContentStorage gameObjectContentStorage)
        {
            _graphics = graphics;
            _camera = camera;
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
                _graphics.PlayAnimation(PredefinedAnimationSid.Death);
            }

            if (!_startToWound)
            {
                _graphics.PlayAnimation(PredefinedAnimationSid.Wound);
                _startToWound = true;
            }
        }

        protected override void DoDraw(SpriteBatch spriteBatch, float zindex)
        {
            base.DoDraw(spriteBatch, zindex);

            _graphics.ShowActiveMarker = false;

            if (_graphics.IsDamaged)
            {
                var allWhite = _gameObjectContentStorage.GetAllWhiteEffect();
                spriteBatch.End();

                spriteBatch.Begin(sortMode: SpriteSortMode.Deferred,
                    blendState: BlendState.AlphaBlend,
                    samplerState: SamplerState.PointClamp,
                    depthStencilState: DepthStencilState.None,
                    rasterizerState: RasterizerState.CullNone,
                    transformMatrix: _camera.GetViewTransformationMatrix(),
                    effect: allWhite);
            }
            else
            {
                spriteBatch.End();

                spriteBatch.Begin(sortMode: SpriteSortMode.Deferred,
                    blendState: BlendState.AlphaBlend,
                    samplerState: SamplerState.PointClamp,
                    depthStencilState: DepthStencilState.None,
                    rasterizerState: RasterizerState.CullNone,
                    transformMatrix: _camera.GetViewTransformationMatrix());
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