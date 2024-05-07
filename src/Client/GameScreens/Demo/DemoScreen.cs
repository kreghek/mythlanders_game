using System;
using System.Collections.Generic;

using Client.Core;
using Client.Engine;
using Client.ScreenManagement;

using CombatDicesTeam.Engine.Ui;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.Demo;

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

        var demoText = StringHelper.LineBreaking(UiResource.DemoText, 60);

        var demoTextSize = _uiContentStorage.GetTitlesFont().MeasureString(demoText);

        spriteBatch.DrawString(_uiContentStorage.GetTitlesFont(), demoText,
            contentRect.Center.ToVector2() - new Vector2(demoTextSize.X / 2, demoTextSize.Y / 2), Color.White);

        _closeButton.Rect =
            new Rectangle(
                new Point(contentRect.Center.X + (int)demoTextSize.X,
                    contentRect.Center.Y + (int)demoTextSize.Y / 2 + ControlBase.CONTENT_MARGIN), new Point(100, 20));
        _closeButton.Draw(spriteBatch);

        spriteBatch.End();
    }

    protected override void InitializeContent()
    {
        _demoBackgroundTexture = Game.Content.Load<Texture2D>("Sprites/Ui/TitleBackground");
    }

    protected override void UpdateContent(GameTime gameTime)
    {
        base.UpdateContent(gameTime);
        
        _closeButton.Update(ResolutionIndependentRenderer);
    }

    private void CloseButton_OnClick(object? sender, EventArgs e)
    {
        ScreenManager.ExecuteTransition(this, ScreenTransition.Title, null!);
    }
}