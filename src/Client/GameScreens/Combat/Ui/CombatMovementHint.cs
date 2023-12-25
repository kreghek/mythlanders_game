using System;

using Client.Core;
using Client.Engine;

using CombatDicesTeam.Combats;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.Combat.Ui;

internal class CombatMovementHint : HintBase
{
    private readonly string? _combatMoveCostText;
    private readonly string _combatMoveDescription;
    private readonly CombatMovementInstance _combatMovement;

    private readonly string _combatMoveTitle;
    private readonly SpriteFont _font;
    private readonly SpriteFont _nameFont;

    public CombatMovementHint(CombatMovementInstance combatMovement)
    {
        _nameFont = UiThemeManager.UiContentStorage.GetTitlesFont();
        _font = UiThemeManager.UiContentStorage.GetMainFont();
        _combatMovement = combatMovement;

        _combatMoveTitle = GameObjectHelper.GetLocalized(_combatMovement.SourceMovement.Sid);

        if (_combatMovement.SourceMovement.Cost.HasCost)
        {
            _combatMoveCostText =
                string.Format(UiResource.SkillManaCostTemplate, _combatMovement.SourceMovement.Cost.Amount);
        }
        else
        {
            _combatMoveCostText = null;
        }

        _combatMoveDescription = StringHelper.LineBreaking(
            GameObjectHelper.GetLocalizedDescription(_combatMovement.SourceMovement.Sid),
            60);

        ContentSize = CalcContentSize(_combatMoveTitle, _combatMoveDescription);
    }

    public Vector2 ContentSize { get; }

    protected override Point CalcTextureOffset()
    {
        return Point.Zero;
    }

    protected override void DrawContent(SpriteBatch spriteBatch, Rectangle clientRect, Color contentColor)
    {
        var color = Color.White;

        var combatMoveTitlePosition = clientRect.Location.ToVector2();

        spriteBatch.DrawString(_nameFont, _combatMoveTitle, combatMoveTitlePosition, color);

        spriteBatch.DrawString(_font,
            string.Format(UiResource.ManaCostLabelTemplate, _combatMovement.Cost.Amount.Current),
            combatMoveTitlePosition + new Vector2(0, 15), color);

        var manaCostPosition = combatMoveTitlePosition + new Vector2(0, 25);
        if (string.IsNullOrEmpty(_combatMoveCostText))
        {
            spriteBatch.DrawString(_font,
                _combatMoveCostText,
                manaCostPosition, color);
        }

        var descriptionBlockPosition = manaCostPosition + new Vector2(0, 20);
        spriteBatch.DrawString(_font, _combatMoveDescription, descriptionBlockPosition, Color.Wheat);
    }

    private Vector2 CalcContentSize(string combatMoveTitle, string combatMoveDescription)
    {
        var titleSize = _nameFont.MeasureString(combatMoveTitle);
        var descriptionSize = _font.MeasureString(combatMoveDescription);

        return new Vector2(
            Math.Max(titleSize.X, descriptionSize.X),
            15 + 20 + 10 + descriptionSize.Y);
    }
}