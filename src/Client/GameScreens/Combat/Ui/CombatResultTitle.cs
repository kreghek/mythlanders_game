using System;

using Client;
using Client.Engine;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.Combat.Ui;

internal sealed class CombatResultTitle : ControlBase
{
    private readonly CombatResult _combatResult;
    private readonly SpriteFont _titleFont;

    public CombatResultTitle(CombatResult combatResult)
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

    private static string GetCombatResultLocalizedText(CombatResult combatResult)
    {
        return combatResult switch
        {
            CombatResult.Victory => UiResource.CombatResultVictoryText,
            CombatResult.Defeat => UiResource.CombatResultDefeatText,
            CombatResult.NextCombat => UiResource.CombatResultNextText,
            _ => throw new ArgumentOutOfRangeException(nameof(combatResult), combatResult, null)
        };
    }
}