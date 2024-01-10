using System;

using Client.Engine;
using Client.ScreenManagement;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.EndGame;

internal sealed class EndGameScreen : GameScreenBase
{
    private readonly ButtonBase _backButton;
    private readonly ICamera2DAdapter _camera;
    private readonly IResolutionIndependentRenderer _resolutionIndependentRenderer;
    private readonly IUiContentStorage _uiContentStorage;

    public EndGameScreen(MythlandersGame game) : base(game)
    {
        _uiContentStorage = game.Services.GetService<IUiContentStorage>();
        _camera = Game.Services.GetService<ICamera2DAdapter>();
        _resolutionIndependentRenderer = Game.Services.GetService<IResolutionIndependentRenderer>();

        _backButton = new ResourceTextButton(nameof(UiResource.CompleteGameButtonTitle));
        _backButton.OnClick += (_, _) =>
        {
            ScreenManager.ExecuteTransition(this, ScreenTransition.Credits, null);
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

        var font = _uiContentStorage.GetTitlesFont();
        var endMessage = string.Format(UiResource.EndOfGameMessage, Environment.NewLine);
        var messageSize = font.MeasureString(endMessage);
        var position = _resolutionIndependentRenderer.VirtualBounds.Center.ToVector2() - messageSize / 2;

        spriteBatch.DrawString(_uiContentStorage.GetMainFont(), endMessage, position, Color.Wheat);

        const int BUTTON_WIDTH = 100;
        const int BUTTON_HEIGHT = 20;
        var buttonPosition = _resolutionIndependentRenderer.VirtualBounds.Center.ToVector2() -
                             Vector2.UnitY * (messageSize.Y / 2 - 10 - BUTTON_HEIGHT) -
                             Vector2.UnitX * BUTTON_WIDTH / 2;

        _backButton.Rect = new Rectangle(buttonPosition.ToPoint(), new Point(BUTTON_WIDTH, BUTTON_HEIGHT));
        _backButton.Draw(spriteBatch);
        spriteBatch.End();
    }

    protected override void InitializeContent()
    {
    }

    protected override void UpdateContent(GameTime gameTime)
    {
        _backButton.Update(_resolutionIndependentRenderer);
    }
}