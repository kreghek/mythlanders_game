using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Engine;
using Rpg.Client.ScreenManagement;

namespace Rpg.Client.GameScreens.Credits
{
    internal sealed class CreditsScreen: GameScreenBase
    {
        private string _creditsText;
        private float _textPosition;
        private readonly IUiContentStorage _uiContentStorage;
        private readonly ResolutionIndependentRenderer _resolutionIndependentRenderer;
        private readonly Camera2D _camera;

        public CreditsScreen(EwarGame game) : base(game)
        {
            _uiContentStorage = game.Services.GetService<IUiContentStorage>();
            _resolutionIndependentRenderer = game.Services.GetService<ResolutionIndependentRenderer>();
            _camera = Game.Services.GetService<Camera2D>();

            _textPosition = _resolutionIndependentRenderer.VirtualHeight + 100;

            _creditsText = "Кредиты";
        }

        protected override void DrawContent(SpriteBatch spriteBatch)
        {
            _resolutionIndependentRenderer.BeginDraw();
            spriteBatch.Begin(
                sortMode: SpriteSortMode.Deferred,
                blendState: BlendState.AlphaBlend,
                samplerState: SamplerState.PointClamp,
                depthStencilState: DepthStencilState.None,
                rasterizerState: RasterizerState.CullNone,
                transformMatrix: _camera.GetViewTransformationMatrix());

            spriteBatch.DrawString(_uiContentStorage.GetTitlesFont(), 
                _creditsText,
                new Vector2(_resolutionIndependentRenderer.VirtualBounds.Center.X, _textPosition), 
                Color.Wheat);

            spriteBatch.End();
        }

        protected override void UpdateContent(GameTime gameTime)
        {
            _textPosition -= (float)gameTime.ElapsedGameTime.TotalSeconds * 100;
        }
    }
}