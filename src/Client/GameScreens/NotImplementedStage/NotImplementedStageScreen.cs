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

internal class NotImplementedStageScreen : GameScreenWithMenuBase
{
    private readonly HeroCampaign _campaign;
    private readonly ButtonBase _skipButton;
    private readonly IUiContentStorage _uiContentStorage;

    public NotImplementedStageScreen(TestamentGame game, NotImplementedStageScreenTransitionArguments args) : base(game)
    {
        _campaign = args.Campaign;

        _uiContentStorage = Game.Services.GetRequiredService<IUiContentStorage>();

        _skipButton = new ResourceTextButton(nameof(UiResource.SkipButtonTitle));
        _skipButton.OnClick += CloseButton_OnClick;
    }

    protected override IList<ButtonBase> CreateMenu()
    {
        var closeButton = new ResourceTextButton(nameof(UiResource.SkipButtonTitle));
        closeButton.OnClick += CloseButton_OnClick;

        return new[]
        {
            closeButton
        };
    }

    protected override void DrawContentWithoutMenu(SpriteBatch spriteBatch, Rectangle contentRect)
    {
        spriteBatch.Begin(sortMode: SpriteSortMode.Deferred,
            blendState: BlendState.AlphaBlend,
            samplerState: SamplerState.PointClamp,
            depthStencilState: DepthStencilState.None,
            rasterizerState: RasterizerState.CullNone,
            transformMatrix: Camera.GetViewTransformationMatrix());

        const string TEXT = "Этап не доступен для демо";

        var size = _uiContentStorage.GetTitlesFont().MeasureString(TEXT);

        spriteBatch.DrawString(_uiContentStorage.GetTitlesFont(), TEXT,
            contentRect.Center.ToVector2() - size, Color.Wheat);

        _skipButton.Rect = new Rectangle(contentRect.Center + new Point((int)size.X + 10), new Point(100, 20));
        _skipButton.Draw(spriteBatch);

        spriteBatch.End();
    }

    protected override void InitializeContent()
    {
    }

    private void CloseButton_OnClick(object? sender, EventArgs e)
    {
        ScreenManager.ExecuteTransition(this, ScreenTransition.Campaign,
            new CampaignScreenTransitionArguments(_campaign));
    }
}