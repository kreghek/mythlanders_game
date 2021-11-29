using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Engine;
using Rpg.Client.ScreenManagement;

namespace Rpg.Client.GameScreens.EndGame
{
    internal sealed class EndGameScreen : GameScreenBase
    {
        private readonly TextButton _backButton;
        private readonly Camera2D _camera;
        private readonly ResolutionIndependentRenderer _resolutionIndependentRenderer;
        private readonly IUiContentStorage _uiContentStorage;

        public EndGameScreen(EwarGame game) : base(game)
        {
            _uiContentStorage = game.Services.GetService<IUiContentStorage>();
            _camera = Game.Services.GetService<Camera2D>();
            _resolutionIndependentRenderer = Game.Services.GetService<ResolutionIndependentRenderer>();

            _backButton = new TextButton("Back", _uiContentStorage.GetButtonTexture(), _uiContentStorage.GetMainFont(),
                Rectangle.Empty);
            _backButton.OnClick += (s, e) =>
            {
                ScreenManager.ExecuteTransition(this, ScreenTransition.Title);
            };
        }

        protected override void DrawContent(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(
                sortMode: SpriteSortMode.Deferred,
                blendState: BlendState.AlphaBlend,
                samplerState: SamplerState.PointClamp,
                depthStencilState: DepthStencilState.None,
                rasterizerState: RasterizerState.CullNone,
                transformMatrix: _camera.GetViewTransformationMatrix());
            spriteBatch.DrawString(_uiContentStorage.GetMainFont(), "Happy end! Or not?", new Vector2(100, 100),
                Color.White);

            _backButton.Rect = new Rectangle(100, 150, 100, 20);
            _backButton.Draw(spriteBatch);
            spriteBatch.End();
        }

        protected override void UpdateContent(GameTime gameTime)
        {
            _backButton.Update(_resolutionIndependentRenderer);
        }
    }
}