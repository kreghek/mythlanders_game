using System.Collections.Generic;
using System.Linq;

using Client.Core.CampaignEffects;
using Client.Engine;
using Client.GameScreens.CampaignReward.Ui;
using Client.GameScreens.Common.CampaignResult;

using CombatDicesTeam.Engine.Ui;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.Common.Result;

internal sealed class CampaignEffectPanel : ControlBase
{
    private readonly SpriteFont _labelFont;
    private readonly IReadOnlyCollection<ICampaignRewardImageDrawer> _rewardImageDrawers;
    private readonly ResultDecoration _resultDecoration;
    private readonly IReadOnlyCollection<ICampaignEffect> _effects;

    public CampaignEffectPanel(IReadOnlyCollection<ICampaignEffect> effects,
        SpriteFont labelFont,
        IReadOnlyCollection<ICampaignRewardImageDrawer> rewardImageDrawers,
        ResultDecoration resultDecoration) : base(UiThemeManager.UiContentStorage
        .GetControlBackgroundTexture())
    {
        _effects = effects;
        _labelFont = labelFont;
        _rewardImageDrawers = rewardImageDrawers;
        _resultDecoration = resultDecoration;
    }

    public void Update(GameTime gameTime)
    {
        foreach (var campaignRewardImageDrawer in _rewardImageDrawers)
        {
            campaignRewardImageDrawer.Update(gameTime);
        }
    }

    protected override Point CalcTextureOffset()
    {
        return ControlTextures.Panel;
    }

    protected override Color CalculateColor()
    {
        return Color.White;
    }

    protected override void DrawContent(SpriteBatch spriteBatch, Rectangle contentRect, Color contentColor)
    {
        if (!_effects.Any())
        {
            return;
        }

        DrawEffectGroupLabel(spriteBatch, contentRect);

        var effects = _effects.ToArray();
        for (var i = 0; i < effects.Length; i++)
        {
            var effect = effects[i];
            var effectPosition = (new Point(contentRect.Center.X, contentRect.Top + 50 + 50 + i * 32)).ToVector2();

            foreach (var campaignRewardImageDrawer in _rewardImageDrawers)
            {
                if (!campaignRewardImageDrawer.IsApplicable(effect))
                {
                    continue;
                }

                var imageSize = campaignRewardImageDrawer.ImageSize;
                campaignRewardImageDrawer.Draw(effect, spriteBatch, effectPosition - new Vector2(imageSize.X, 0));
            }
        }
    }

    private void DrawEffectGroupLabel(SpriteBatch spriteBatch, Rectangle contentRect)
    {
        var labelText = GetLabelText(_resultDecoration);
        var labelSize = _labelFont.MeasureString(labelText);
        spriteBatch.DrawString(_labelFont, labelText,
            new Vector2(contentRect.Center.X - labelSize.X / 2, contentRect.Top + 50), Color.Wheat);
    }

    private static string GetLabelText(ResultDecoration resultDecoration)
    {
        return resultDecoration switch
        {
            ResultDecoration.Victory => UiResource.CampaignRewardsLabelText,
            ResultDecoration.Defeat => UiResource.CampaignPenaltiesLabelText,
            _ => string.Empty
        };
    }
}