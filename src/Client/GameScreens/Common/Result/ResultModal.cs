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
    private readonly ButtonBase _closeButton;
    private readonly RewardPanel _combatRewardList;
    private readonly LinearAnimationFrameSet _flagAnimation;
    private readonly Texture2D _flagTexture;
    private readonly ResultModalTitle _title;
    private double _iterationCounter;

    public ResultModal(IUiContentStorage uiContentStorage,
        IResolutionIndependentRenderer resolutionIndependentRenderer,
        ResultDecoration combatResult,
        IReadOnlyCollection<ICampaignEffect> rewards,
        Texture2D flagTexture,
        ICampaignRewardImageDrawer[] drawers) : base(uiContentStorage, resolutionIndependentRenderer)
    {
        CombatResult = combatResult;
        _flagTexture = flagTexture;
        _closeButton = new ResourceTextButton(nameof(UiResource.CloseButtonTitle));
        _closeButton.OnClick += CloseButton_OnClick;

        _title = new ResultModalTitle(combatResult);

        _combatRewardList = new RewardPanel(rewards,
            uiContentStorage.GetCombatSkillPanelTexture(),
            uiContentStorage.GetMainFont(),
            uiContentStorage.GetMainFont(),
            drawers
        );

        _flagAnimation =
            new LinearAnimationFrameSet(Enumerable.Range(0, 8).ToArray(), 8, flagTexture.Width / 4,
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

        var benefitsPosition = new Vector2(
            ContentRect.Location.X + ControlBase.CONTENT_MARGIN,
            _title.Rect.Bottom + ControlBase.CONTENT_MARGIN);
        var benefitsRect = new Rectangle(benefitsPosition.ToPoint(),
            new Point(ContentRect.Width, ContentRect.Height - _title.Rect.Height - ControlBase.CONTENT_MARGIN));

        DrawVictoryAftermaths(spriteBatch, benefitsRect);

        spriteBatch.Draw(_flagTexture, new Vector2(ContentRect.Left, ContentRect.Top), _flagAnimation.GetFrameRect(),
            Color.White);
        spriteBatch.Draw(_flagTexture, new Vector2(ContentRect.Right - 41, ContentRect.Top),
            _flagAnimation.GetFrameRect(), Color.White);

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
            _combatRewardList.Update(gameTime);
            _iterationCounter = 0;
        }

        _closeButton.Update(screenProjection);

        _flagAnimation.Update(gameTime);
    }

    private void CloseButton_OnClick(object? sender, EventArgs e)
    {
        Close();
    }

    private void DrawVictoryAftermaths(SpriteBatch spriteBatch, Rectangle benefitsRect)
    {
        _combatRewardList.Rect = benefitsRect;
        _combatRewardList.Draw(spriteBatch);
    }
}