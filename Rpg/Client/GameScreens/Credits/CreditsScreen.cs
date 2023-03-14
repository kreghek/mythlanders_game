using Client;
using Client.Engine;

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
        private readonly string _creditsText;
        private readonly SpriteFont _font;
        private readonly ResolutionIndependentRenderer _resolutionIndependentRenderer;
        private readonly IUiContentStorage _uiContentStorage;
        private float _textPosition;

        public CreditsScreen(TestamentGame game) : base(game)
        {
            _uiContentStorage = game.Services.GetService<IUiContentStorage>();
            _resolutionIndependentRenderer = game.Services.GetService<ResolutionIndependentRenderer>();
            _camera = Game.Services.GetService<Camera2D>();

            _textPosition = _resolutionIndependentRenderer.VirtualHeight + 100;

            _creditsText = CreditsResource.ResourceManager.GetString("Credits") ?? string.Empty;

            _font = _uiContentStorage.GetTitlesFont();

            _backButton = new ResourceTextButton(nameof(UiResource.BackButtonTitle));

            _backButton.OnClick += (_, _) => { ScreenManager.ExecuteTransition(this, ScreenTransition.Title, null); };
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

            spriteBatch.DrawString(_font,
                _creditsText,
                new Vector2(_resolutionIndependentRenderer.VirtualBounds.Center.X, _textPosition),
                Color.Wheat);

            _backButton.Rect = new Rectangle(5, 5, 100, 20);
            _backButton.Draw(spriteBatch);

            spriteBatch.End();
        }

        protected override void InitializeContent()
        {
        }

        protected override void UpdateContent(GameTime gameTime)
        {
            var size = _font.MeasureString(_creditsText);

            const int TEXT_SPEED = 100;
            const int DELAY_SECONDS = 2;

            _textPosition -= (float)gameTime.ElapsedGameTime.TotalSeconds * TEXT_SPEED;

            _backButton.Update(_resolutionIndependentRenderer);

            if (_textPosition <= -(size.Y + DELAY_SECONDS * TEXT_SPEED))
            {
                ScreenManager.ExecuteTransition(this, ScreenTransition.Title, null);
            }
        }
    }
}