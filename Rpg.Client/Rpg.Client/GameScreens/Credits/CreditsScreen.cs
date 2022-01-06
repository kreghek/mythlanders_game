using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Engine;
using Rpg.Client.ScreenManagement;

namespace Rpg.Client.GameScreens.Credits
{
    internal sealed class CreditsScreen : GameScreenBase
    {
        private readonly ResourceTextButton _backButton;
        private readonly Camera2D _camera;
        private readonly ResolutionIndependentRenderer _resolutionIndependentRenderer;
        private readonly IUiContentStorage _uiContentStorage;
        private readonly string _creditsText;
        private float _textPosition;

        public CreditsScreen(EwarGame game) : base(game)
        {
            _uiContentStorage = game.Services.GetService<IUiContentStorage>();
            _resolutionIndependentRenderer = game.Services.GetService<ResolutionIndependentRenderer>();
            _camera = Game.Services.GetService<Camera2D>();

            _textPosition = _resolutionIndependentRenderer.VirtualHeight + 100;

            _creditsText = CreditsResource.ResourceManager.GetString("Credits");

            _backButton = new ResourceTextButton(nameof(UiResource.BackButtonTitle),
                _uiContentStorage.GetButtonTexture(), _uiContentStorage.GetMainFont(), Rectangle.Empty);
            _backButton.OnClick += (_, _) => { ScreenManager.ExecuteTransition(this, ScreenTransition.Title); };
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

            _backButton.Rect = new Rectangle(5, 5, 100, 20);
            _backButton.Draw(spriteBatch);

            spriteBatch.End();
        }

        protected override void UpdateContent(GameTime gameTime)
        {
            _textPosition -= (float)gameTime.ElapsedGameTime.TotalSeconds * 100;

            _backButton.Update(_resolutionIndependentRenderer);
        }
    }
}