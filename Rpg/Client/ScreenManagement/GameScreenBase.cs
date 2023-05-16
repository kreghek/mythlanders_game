using System.Collections.Generic;
using System.Linq;

using Client;
using Client.Engine;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Engine;

namespace Rpg.Client.ScreenManagement
{
    /// <summary>
    /// This is the base class for all game scenes.
    /// </summary>
    internal abstract class GameScreenBase : EwarRenderableBase, IScreen
    {
        private readonly IList<IModalWindow> _modals;

        private bool _isInitialized;

        public GameScreenBase(TestamentGame game)
        {
            Game = game;

            ScreenManager = game.Services.GetService<IScreenManager>();

            Camera = Game.Services.GetService<ICamera2DAdapter>();
            ResolutionIndependentRenderer = Game.Services.GetService<IResolutionIndependentRenderer>();

            _modals = new List<IModalWindow>();
        }

        public TestamentGame Game { get; }
        public IScreenManager ScreenManager { get; }
        protected ICamera2DAdapter Camera { get; }
        protected IResolutionIndependentRenderer ResolutionIndependentRenderer { get; }

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

            if (_isInitialized)
            {
                DrawContent(spriteBatch);
            }

            DrawModals(spriteBatch);
        }

        protected abstract void DrawContent(SpriteBatch spriteBatch);

        protected abstract void InitializeContent();

        protected abstract void UpdateContent(GameTime gameTime);

        protected void UpdateModals(GameTime gameTime)
        {
            foreach (var modal in _modals)
            {
                if (modal.IsVisible)
                {
                    modal.Update(gameTime, ResolutionIndependentRenderer);
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
                transformMatrix: Camera.GetViewTransformationMatrix());

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
                if (_isInitialized)
                {
                    UpdateContent(gameTime);
                }
                else
                {
                    InitializeContent();
                    _isInitialized = true;
                }
            }

            UpdateModals(gameTime);
        }
    }
}