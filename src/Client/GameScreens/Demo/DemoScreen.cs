using System;
using System.Collections.Generic;

using Client.Core.Campaigns;
using Client.Engine;
using Client.GameScreens.Campaign;
using Client.ScreenManagement;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.NotImplementedStage;

internal class DemoScreen : GameScreenWithMenuBase
{
    private readonly ButtonBase _closeButton;
    private readonly IUiContentStorage _uiContentStorage;
    private Texture2D? _demoBackgroundTexture;

    public DemoScreen(MythlandersGame game) :
        base(game)
    {
        _uiContentStorage = Game.Services.GetRequiredService<IUiContentStorage>();

        _closeButton = new ResourceTextButton(nameof(UiResource.CloseButtonTitle));
        _closeButton.OnClick += CloseButton_OnClick;
    }

    protected override IList<ButtonBase> CreateMenu()
    {
        return Array.Empty<ButtonBase>();
    }

    protected override void DrawContentWithoutMenu(SpriteBatch spriteBatch, Rectangle contentRect)
    {
        spriteBatch.Begin(sortMode: SpriteSortMode.Deferred,
            blendState: BlendState.AlphaBlend,
            samplerState: SamplerState.PointClamp,
            depthStencilState: DepthStencilState.None,
            rasterizerState: RasterizerState.CullNone,
            transformMatrix: Camera.GetViewTransformationMatrix());

        spriteBatch.Draw(_demoBackgroundTexture, contentRect, Color.White);

        var text = UiResource.DemoText;

        var size = _uiContentStorage.GetTitlesFont().MeasureString(text);

        spriteBatch.DrawString(_uiContentStorage.GetTitlesFont(), text,
            contentRect.Center.ToVector2() - size, Color.Wheat);

        _closeButton.Rect = new Rectangle(contentRect.Center + new Point((int)size.X + 10), new Point(100, 20));
        _closeButton.Draw(spriteBatch);

        spriteBatch.End();
    }

    protected override void InitializeContent()
    {
        _demoBackgroundTexture = Game.Content.Load<Texture2D>("Sprites/Ui/TitleBackground");
    }

    private void CloseButton_OnClick(object? sender, EventArgs e)
    {
        ScreenManager.ExecuteTransition(this, ScreenTransition.Title, null!);
    }
}