using Client.Engine;
using Client.ScreenManagement;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.Credits;

internal sealed class CreditsScreen : GameScreenBase
{
    private readonly ResourceTextButton _backButton;
    private readonly ICamera2DAdapter _camera;
    private readonly string _creditsText;
    private readonly SpriteFont _font;
    private readonly IResolutionIndependentRenderer _resolutionIndependentRenderer;
    private readonly IUiContentStorage _uiContentStorage;
    private float _contentScrollProgress;

    private Texture2D? _smallLogoTexture;

    public CreditsScreen(TestamentGame game) : base(game)
    {
        _uiContentStorage = game.Services.GetService<IUiContentStorage>();
        _resolutionIndependentRenderer = game.Services.GetService<IResolutionIndependentRenderer>();
        _camera = Game.Services.GetService<ICamera2DAdapter>();

        _contentScrollProgress = _resolutionIndependentRenderer.VirtualBounds.Height + 100;

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

        var logoPosition =
            new Vector2(_resolutionIndependentRenderer.VirtualBounds.Center.X - _smallLogoTexture!.Width / 2,
                _contentScrollProgress);
        spriteBatch.Draw(_smallLogoTexture!, logoPosition, Color.White);

        if (VersionHelper.TryReadVersion(out var version))
        {
            spriteBatch.DrawString(_font, $"Version: {version}",
                new Vector2(_resolutionIndependentRenderer.VirtualBounds.Center.X,
                    _contentScrollProgress + _smallLogoTexture!.Height + 10),
                Color.White);
        }

        var authorText = $"Author\n{CreditsResource.Author}";
        var authorSize = _font.MeasureString(authorText);
        spriteBatch.DrawString(_font, authorText,
            new Vector2(_resolutionIndependentRenderer.VirtualBounds.Center.X - authorSize.X / 2,
                _contentScrollProgress + _smallLogoTexture!.Height + 100),
            Color.Wheat);

        var creditsSize = _font.MeasureString(_creditsText);
        spriteBatch.DrawString(_font,
            _creditsText,
            new Vector2(_resolutionIndependentRenderer.VirtualBounds.Center.X - creditsSize.X / 2,
                _contentScrollProgress + _smallLogoTexture!.Height + 150),
            Color.Wheat);

        _backButton.Rect = new Rectangle(5, 5, 100, 20);
        _backButton.Draw(spriteBatch);

        spriteBatch.End();
    }

    protected override void InitializeContent()
    {
        _smallLogoTexture = Game.Content.Load<Texture2D>("Sprites/Ui/SmallLogo");
    }

    protected override void UpdateContent(GameTime gameTime)
    {
        var size = _font.MeasureString(_creditsText);

        const int TEXT_SPEED = 75;
        const int DELAY_SECONDS = 2;

        _contentScrollProgress -= (float)gameTime.ElapsedGameTime.TotalSeconds * TEXT_SPEED;

        _backButton.Update(_resolutionIndependentRenderer);

        const int COMPENSATION = 250; //TODO Calculate size of content include logo, version and author
        if (_contentScrollProgress <= -(size.Y + DELAY_SECONDS * TEXT_SPEED + COMPENSATION))
        {
            ScreenManager.ExecuteTransition(this, ScreenTransition.Title, null);
        }
    }
}