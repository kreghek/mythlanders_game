using System;

using Core.Combats;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Engine;
using Rpg.Client.GameScreens;

namespace Client.GameScreens.Combat.Ui;

internal class CombatMovementHint : HintBase
{
    private readonly SpriteFont _nameFont;
    private readonly SpriteFont _font;
    private readonly CombatMovementInstance _combatMovement;

    private readonly string _combatMoveTitle;
    private readonly string? _combatMoveCostText;
    private readonly string _combatMoveDescription;
    private readonly Vector2 _contentSize;

    public CombatMovementHint(CombatMovementInstance combatMovement)
    {
        _nameFont = UiThemeManager.UiContentStorage.GetTitlesFont();
        _font = UiThemeManager.UiContentStorage.GetMainFont();
        _combatMovement = combatMovement;

        _combatMoveTitle = GameObjectHelper.GetLocalized(_combatMovement.SourceMovement.Sid);

        if (_combatMovement.SourceMovement.Cost.HasCost)
        {
            _combatMoveCostText = string.Format(UiResource.SkillManaCostTemplate, _combatMovement.SourceMovement.Cost.Value);
        }
        else
        {
            _combatMoveCostText = null;
        }

        _combatMoveDescription = StringHelper.LineBreaking(
            GameObjectHelper.GetLocalizedDescription(_combatMovement.SourceMovement.Sid),
            60);

        _contentSize = CalcContentSize(_combatMoveTitle, _combatMoveDescription);
    }

    private Vector2 CalcContentSize(string combatMoveTitle, string combatMoveDescription)
    {
        var titleSize = _nameFont.MeasureString(combatMoveTitle);
        var descriptionSize = _font.MeasureString(combatMoveDescription);

        return new Vector2(
            Math.Max(titleSize.X, descriptionSize.X),
            15 + 20 + descriptionSize.Y);
    }

    protected override Point CalcTextureOffset()
    {
        return Point.Zero;
    }

    public Vector2 ContentSize => _contentSize;

    protected override void DrawContent(SpriteBatch spriteBatch, Rectangle clientRect, Color contentColor)
    {
        var color = Color.White;

        var combatMoveTitlePosition = clientRect.Location.ToVector2();

        spriteBatch.DrawString(_nameFont, _combatMoveTitle, combatMoveTitlePosition, color);

        var manaCostPosition = combatMoveTitlePosition + new Vector2(0, 15);
        if (string.IsNullOrEmpty(_combatMoveCostText))
        {
            spriteBatch.DrawString(_font,
                _combatMoveCostText,
                manaCostPosition, color);
        }

        var descriptionBlockPosition = manaCostPosition + new Vector2(0, 20);
        spriteBatch.DrawString(_font, _combatMoveDescription, descriptionBlockPosition, Color.Wheat);
    }
}