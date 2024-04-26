using System;
using System.Collections.Generic;
using System.Linq;

using Client.Core.CampaignEffects;
using Client.Engine;
using Client.GameScreens.CampaignReward.Ui;
using Client.GameScreens.Combat;
using Client.GameScreens.Combat.Ui;
using Client.GameScreens.Common.CampaignResult;

using CombatDicesTeam.Engine.Ui;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.Common.Result;

internal sealed class ResultModal : ModalDialogBase
{
    private readonly ButtonBase _closeButton;
    private readonly RewardPanel _combatRewardList;

    private readonly CombatResultTitle _title;

    private double _iterationCounter;

    public ResultModal(IUiContentStorage uiContentStorage,
        GameObjectContentStorage gameObjectContentStorage,
        IResolutionIndependentRenderer resolutionIndependentRenderer,
        ResultDecoration combatResult,
        IReadOnlyCollection<ICampaignEffect> rewards,
        ICampaignRewardImageDrawer[] drawers) : base(uiContentStorage, resolutionIndependentRenderer)
    {
        CombatResult = combatResult;
        _closeButton = new ResourceTextButton(nameof(UiResource.CloseButtonTitle));
        _closeButton.OnClick += CloseButton_OnClick;

        _title = new CombatResultTitle(combatResult);

        _combatRewardList = new RewardPanel(rewards,
            uiContentStorage.GetCombatSkillPanelTexture(),
            uiContentStorage.GetMainFont(),
            uiContentStorage.GetMainFont(), 
            drawers
        );
    }

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