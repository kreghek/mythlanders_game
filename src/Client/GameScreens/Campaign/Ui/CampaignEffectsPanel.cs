using System.Collections.Generic;

using Client.Core.CampaignEffects;
using Client.Engine;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.Campaign.Ui;

internal class CampaignEffectsPanel : ControlBase
{
    private const int CAMPAIGN_EFFECT_TEXT_HEIGHT = 20;
    private readonly IReadOnlyCollection<ICampaignEffect> _estimatedPenalties;

    private readonly IReadOnlyCollection<ICampaignEffect> _estimatedRewards;

    public CampaignEffectsPanel(IReadOnlyCollection<ICampaignEffect> estimatedRewards,
        IReadOnlyCollection<ICampaignEffect> estimatedPenalties) : base(UiThemeManager.UiContentStorage.GetControlBackgroundTexture())
    {
        _estimatedRewards = estimatedRewards;
        _estimatedPenalties = estimatedPenalties;
    }

    protected override Point CalcTextureOffset()
    {
        return ControlTextures.PanelBlack;
    }

    protected override Color CalculateColor()
    {
        return Color.White;
    }

    protected override void DrawContent(SpriteBatch spriteBatch, Rectangle contentRect, Color contentColor)
    {
        spriteBatch.DrawString(UiThemeManager.UiContentStorage.GetMainFont(),
            UiResource.CampaignRewardsLabelText + ":",
            new Vector2(contentRect.Left + CONTENT_MARGIN,
                contentRect.Top + CONTENT_MARGIN),
            Color.Wheat);

        var currentBottomY = contentRect.Top + CONTENT_MARGIN + CAMPAIGN_EFFECT_TEXT_HEIGHT + CONTENT_MARGIN;
        foreach (var effect in _estimatedRewards)
        {
            spriteBatch.DrawString(
                UiThemeManager.UiContentStorage.GetMainFont(),
                effect.GetEffectDisplayText(),
                new Vector2(contentRect.Left + CONTENT_MARGIN, currentBottomY),
                Color.White);
            currentBottomY += CONTENT_MARGIN + CAMPAIGN_EFFECT_TEXT_HEIGHT;
        }

        spriteBatch.DrawString(UiThemeManager.UiContentStorage.GetMainFont(),
            UiResource.CampaignPenaltiesLabelText + ":",
            new Vector2(contentRect.Left + CONTENT_MARGIN,
                currentBottomY),
            Color.Wheat);

        currentBottomY += CONTENT_MARGIN + CAMPAIGN_EFFECT_TEXT_HEIGHT;

        foreach (var effect in _estimatedPenalties)
        {
            spriteBatch.DrawString(
                UiThemeManager.UiContentStorage.GetMainFont(),
                effect.GetEffectDisplayText(),
                new Vector2(contentRect.Left + CONTENT_MARGIN, currentBottomY),
                Color.White);
            currentBottomY += CONTENT_MARGIN + CAMPAIGN_EFFECT_TEXT_HEIGHT;
        }
    }
}