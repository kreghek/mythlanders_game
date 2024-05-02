using System;
using System.Collections.Generic;
using System.Linq;

using Client.Core.CampaignEffects;
using Client.Engine;
using Client.GameScreens.CampaignReward.Ui;
using Client.GameScreens.Common.CampaignResult;

using CombatDicesTeam.Engine.Ui;

using GameClient.Engine.Animations;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.Common.Result;

internal sealed class ResultModal : ModalDialogBase
{
    private readonly CampaignEffectPanel _campaignEffectPanel;
    private readonly ButtonBase _closeButton;
    private readonly LinearAnimationFrameSet _flagAnimationLeft;
    private readonly LinearAnimationFrameSet _flagAnimationRight;
    private readonly Texture2D _flagTexture;
    private readonly ResultModalTitle _title;
    private double _iterationCounter;

    public ResultModal(IUiContentStorage uiContentStorage,
        IResolutionIndependentRenderer resolutionIndependentRenderer,
        ResultDecoration resultDecoration,
        IReadOnlyCollection<ICampaignEffect> rewards,
        Texture2D flagTexture,
        ICampaignRewardImageDrawer[] drawers) : base(uiContentStorage, resolutionIndependentRenderer)
    {
        CombatResult = resultDecoration;
        _flagTexture = flagTexture;
        _closeButton = new ResourceTextButton(nameof(UiResource.CloseButtonTitle));
        _closeButton.OnClick += CloseButton_OnClick;

        _title = new ResultModalTitle(resultDecoration);

        _campaignEffectPanel = new CampaignEffectPanel(rewards,
            uiContentStorage.GetMainFont(),
            drawers,
            resultDecoration
        );

        _flagAnimationLeft =
            new LinearAnimationFrameSet(Enumerable.Range(0, 8).ToArray(), 8, flagTexture.Width / 4,
                flagTexture.Height / 2, 4) { IsLooping = true };

        _flagAnimationRight =
            new LinearAnimationFrameSet(Enumerable.Range(0, 8).ToArray(), 9, flagTexture.Width / 4,
                flagTexture.Height / 2, 4) { IsLooping = true };
    }

    protected override ModalTopSymbol? TopSymbol => CombatResult == ResultDecoration.Victory
        ? ModalTopSymbol.CombatResultVictory
        : ModalTopSymbol.CombatResultDefeat;

    internal ResultDecoration CombatResult { get; }

    protected override void DrawContent(SpriteBatch spriteBatch)
    {
        _title.Rect = new Rectangle(ContentRect.Location, new Point(ContentRect.Width, 50));
        _title.Draw(spriteBatch);

        var effectsPosition = new Vector2(
            ContentRect.Left + ControlBase.CONTENT_MARGIN,
            _title.Rect.Bottom + ControlBase.CONTENT_MARGIN);
        var effectsRect = new Rectangle(effectsPosition.ToPoint(),
            new Point(ContentRect.Width, ContentRect.Height - _title.Rect.Height - ControlBase.CONTENT_MARGIN));

        DrawEffects(spriteBatch, effectsRect);

        DrawFlags(spriteBatch);

        _closeButton.Rect = new Rectangle(ContentRect.Center.X - 50, ContentRect.Bottom - 25, 100, 20);
        _closeButton.Draw(spriteBatch);
    }

    protected override void UpdateContent(GameTime gameTime,
        IScreenProjection screenProjection)
    {
        base.UpdateContent(gameTime, screenProjection);

        _iterationCounter += gameTime.ElapsedGameTime.TotalSeconds;

        if (_iterationCounter >= 0.01)
        {
            _campaignEffectPanel.Update(gameTime);
            _iterationCounter = 0;
        }

        _closeButton.Update(screenProjection);

        UpdateFlags(gameTime);
    }

    private void CloseButton_OnClick(object? sender, EventArgs e)
    {
        Close();
    }

    private void DrawEffects(SpriteBatch spriteBatch, Rectangle benefitsRect)
    {
        _campaignEffectPanel.Rect = benefitsRect;
        _campaignEffectPanel.Draw(spriteBatch);
    }

    private void DrawFlags(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_flagTexture,
            new Vector2(ContentRect.Left, ContentRect.Top),
            _flagAnimationLeft.GetFrameRect(),
            Color.White);

        spriteBatch.Draw(_flagTexture,
            new Vector2(ContentRect.Right - _flagTexture.Width, ContentRect.Top),
            _flagAnimationRight.GetFrameRect(),
            Color.White);
    }

    private void UpdateFlags(GameTime gameTime)
    {
        _flagAnimationLeft.Update(gameTime);
        _flagAnimationRight.Update(gameTime);
    }
}