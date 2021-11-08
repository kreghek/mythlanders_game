using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Engine;

namespace Rpg.Client.Screens
{
    /// <summary>
    /// This is the base class for all game scenes.
    /// </summary>
    internal abstract class GameScreenBase : EwarDrawableComponentBase, IScreen
    {
        private readonly Camera2D _camera;
        private readonly IList<IModalWindow> _modals;
        private readonly ResolutionIndependentRenderer _resolutionIndependenceRenderer;

        public GameScreenBase(EwarGame game)
        {
            Game = game;

            ScreenManager = game.Services.GetService<IScreenManager>();

            _camera = Game.Services.GetService<Camera2D>();
            _resolutionIndependenceRenderer = Game.Services.GetService<ResolutionIndependentRenderer>();

            _modals = new List<IModalWindow>();
        }

        public EwarGame Game { get; }
        public IScreenManager ScreenManager { get; }

        protected void AddModal(IModalWindow modal, bool isLate)
        {
            _modals.Add(modal);
            if (!isLate)
            {
                modal.Show();
            }
        }

        protected override void DoDraw(SpriteBatch spriteBatch, float zindex)
        {
            base.DoDraw(spriteBatch, zindex);

            DrawContent(spriteBatch);

            DrawModals(spriteBatch);
        }

        protected abstract void DrawContent(SpriteBatch spriteBatch);

        protected abstract void UpdateContent(GameTime gameTime);

        protected void UpdateModals(GameTime gameTime)
        {
            foreach (var modal in _modals)
            {
                if (modal.IsVisible)
                {
                    modal.Update(gameTime, _resolutionIndependenceRenderer);
                    break;
                }
            }
        }

        private void DrawModals(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(
                sortMode: SpriteSortMode.Deferred,
                blendState: BlendState.AlphaBlend,
                samplerState: SamplerState.PointClamp,
                depthStencilState: DepthStencilState.None,
                rasterizerState: RasterizerState.CullNone,
                transformMatrix: _camera.GetViewTransformationMatrix());

            foreach (var modal in _modals)
            {
                if (modal.IsVisible)
                {
                    modal.Draw(spriteBatch);
                    break;
                }
            }

            spriteBatch.End();
        }

        public IScreen? TargetScreen { get; set; }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (!_modals.Any(x => x.IsVisible))
            {
                UpdateContent(gameTime);
            }

            UpdateModals(gameTime);
        }
    }
}