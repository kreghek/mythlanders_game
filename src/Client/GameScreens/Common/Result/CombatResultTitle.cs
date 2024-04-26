using System;

using Client.Engine;
using Client.GameScreens.Common.CampaignResult;

using CombatDicesTeam.Engine.Ui;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.Common.Result;

internal sealed class CombatResultTitle : ControlBase
{
    private readonly ResultDecoration _combatResult;
    private readonly SpriteFont _titleFont;

    public CombatResultTitle(ResultDecoration combatResult) : base(UiThemeManager.UiContentStorage
        .GetControlBackgroundTexture())
    {
        _titleFont = UiThemeManager.UiContentStorage.GetTitlesFont();
        _combatResult = combatResult;
    }

    protected override Point CalcTextureOffset()
    {
        return Point.Zero;
    }

    protected override Color CalculateColor()
    {
        return Color.White;
    }

    protected override void DrawBackground(SpriteBatch spriteBatch, Color color)
    {
        // Do not draw background
    }

    protected override void DrawContent(SpriteBatch spriteBatch, Rectangle contentRect, Color contentColor)
    {
        var localizedCombatResultText = GetCombatResultLocalizedText(_combatResult);
        var resultTitleFont = _titleFont;
        var resultTitleSize = resultTitleFont.MeasureString(localizedCombatResultText);

        var titlePosition = new Vector2(contentRect.Center.X, contentRect.Center.Y) -
                            new Vector2(resultTitleSize.X / 2, resultTitleSize.Y / 2);

        spriteBatch.DrawString(resultTitleFont, localizedCombatResultText, titlePosition,
            Color.Wheat);
    }

    private static string GetCombatResultLocalizedText(ResultDecoration combatResult)
    {
        return combatResult switch
        {
            ResultDecoration.Victory => UiResource.CombatResultVictoryText,
            ResultDecoration.Defeat => UiResource.CombatResultDefeatText,
            _ => throw new ArgumentOutOfRangeException(nameof(combatResult), combatResult, null)
        };
    }
}