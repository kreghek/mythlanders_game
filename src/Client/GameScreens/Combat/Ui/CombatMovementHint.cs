using System;

using Client.Core;
using Client.Engine;

using CombatDicesTeam.Combats;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.Combat.Ui;

internal class CombatMovementHint : HintBase
{
    private readonly string _combatMoveDescription;
    private readonly CombatMovementInstance _combatMovement;
    private readonly IStatValue _currentActorResolveValue;
    private readonly string _combatMoveTitle;
    private readonly SpriteFont _descriptionTextFont;
    private readonly SpriteFont _nameTextFont;
    private readonly SpriteFont _costTextFont;

    public CombatMovementHint(CombatMovementInstance combatMovement, IStatValue currentActorResolveValue)
    {
        _nameTextFont = UiThemeManager.UiContentStorage.GetTitlesFont();
        _descriptionTextFont = UiThemeManager.UiContentStorage.GetMainFont();
        _costTextFont = UiThemeManager.UiContentStorage.GetMainFont();
        _combatMovement = combatMovement;
        _currentActorResolveValue = currentActorResolveValue;

        _combatMoveTitle = GameObjectHelper.GetLocalized(_combatMovement.SourceMovement.Sid);

        var values = GetCombatMovementValues(combatMovement);

        _combatMoveDescription = StringHelper.LineBreaking(
            string.Format(GameObjectHelper.GetLocalizedDescription(_combatMovement.SourceMovement.Sid), values),
            60);

        ContentSize = CalcContentSize(_combatMoveTitle, _combatMoveDescription);
    }

    private object[] GetCombatMovementValues(CombatMovementInstance combatMovement)
    {
        throw new NotImplementedException();
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

        spriteBatch.DrawString(_nameTextFont, _combatMoveTitle, combatMoveTitlePosition, color);

        var resolveColor = color;
        if (_combatMovement.Cost.Amount.Current > _currentActorResolveValue.Current)
        {
            resolveColor = Color.Lerp(color, MythlandersColors.DangerRedMain, 0.75f);
        }

        spriteBatch.DrawString(_descriptionTextFont,
            string.Format(UiResource.CombatMovementCostLabelTemplate, _combatMovement.Cost.Amount.Current, _currentActorResolveValue.Current),
            combatMoveTitlePosition + new Vector2(0, 15), resolveColor);

        var manaCostPosition = combatMoveTitlePosition + new Vector2(0, 25);
        var descriptionBlockPosition = manaCostPosition + new Vector2(0, 20);
        spriteBatch.DrawString(_descriptionTextFont, _combatMoveDescription, descriptionBlockPosition, Color.Wheat);
    }

    private Vector2 CalcContentSize(string combatMoveTitle, string combatMoveDescription)
    {
        var titleSize = _nameTextFont.MeasureString(combatMoveTitle);
        var descriptionSize = _descriptionTextFont.MeasureString(combatMoveDescription);

        return new Vector2(
            Math.Max(titleSize.X, descriptionSize.X),
            15 + 20 + 10 + descriptionSize.Y);
    }
}