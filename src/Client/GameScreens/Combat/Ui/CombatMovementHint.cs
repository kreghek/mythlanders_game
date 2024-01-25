using System;
using System.Collections.Generic;

using Client.Assets.CombatMovements;
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
    private readonly ICombatMovementVisualizationProvider _combatMovementVisualizationProvider;
    private readonly string _combatMoveTitle;
    private readonly SpriteFont _descriptionTextFont;
    private readonly SpriteFont _nameTextFont;
    private readonly SpriteFont _costTextFont;

    public CombatMovementHint(CombatMovementInstance combatMovement, IStatValue currentActorResolveValue, ICombatMovementVisualizationProvider combatMovementVisualizationProvider)
    {
        _nameTextFont = UiThemeManager.UiContentStorage.GetTitlesFont();
        _descriptionTextFont = UiThemeManager.UiContentStorage.GetMainFont();
        _costTextFont = UiThemeManager.UiContentStorage.GetTitlesFont();
        _combatMovement = combatMovement;
        _currentActorResolveValue = currentActorResolveValue;
        _combatMovementVisualizationProvider = combatMovementVisualizationProvider;
        _combatMoveTitle = GameObjectHelper.GetLocalized(_combatMovement.SourceMovement.Sid);

        var combatMovementDisplayValues = ExtractCombatMovementValues(combatMovement);

        var combatMovementSid = _combatMovement.SourceMovement.Sid;
        _combatMoveDescription = StringHelper.LineBreaking(
            RenderDescriptionText(combatMovementDisplayValues, combatMovementSid),
            60);

        ContentSize = CalcContentSize(_combatMoveTitle, _combatMoveDescription);
    }

    private static string RenderDescriptionText(IReadOnlyList<CombatMovementEffectDisplayValue> values, CombatMovementSid combatMovementSid)
    {
        var descriptionMarkupText = GameObjectHelper.GetLocalizedDescription(combatMovementSid);

        foreach (var value in values)
        {
            descriptionMarkupText = descriptionMarkupText.Replace($"<{value.Tag}>", GetValueText(value));
        }

        return descriptionMarkupText;
    }

    private static string GetValueText(CombatMovementEffectDisplayValue value)
    {
        var template = GetValueTemplate(value.Template);
        return string.Format(template, value.Value);
    }

    private static string GetValueTemplate(CombatMovementEffectDisplayValueTemplate valueType)
    {
        return valueType switch
        {
            CombatMovementEffectDisplayValueTemplate.Damage =>
                UiResource.CombatMovementEffectValueType_Damage_Template,
            CombatMovementEffectDisplayValueTemplate.DamageModifier =>
                UiResource.CombatMovementEffectValueType_DamageModifer_Template,
            CombatMovementEffectDisplayValueTemplate.RoundDuration =>
                UiResource.CombatMovementEffectValueType_RoundDuration_Template,
            CombatMovementEffectDisplayValueTemplate.ResolveDamage =>
                UiResource.CombatMovementEffectValueType_ResolveDamage_Template,
            CombatMovementEffectDisplayValueTemplate.HitPoints =>
                UiResource.CombatMovementEffectValueType_HitPoints_Template,
            CombatMovementEffectDisplayValueTemplate.ShieldPoints =>
                UiResource.CombatMovementEffectValueType_ShieldPoints_Template,
            _ => "<{0}> units",
        };
    }

    private IReadOnlyList<CombatMovementEffectDisplayValue> ExtractCombatMovementValues(CombatMovementInstance combatMovement)
    {
        return _combatMovementVisualizationProvider.ExtractCombatMovementValues(combatMovement);
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

        var resolveColor = Color.Lerp(color, MythlandersColors.MaxDark, 0.5f);
        if (_combatMovement.Cost.Amount.Current > _currentActorResolveValue.Current)
        {
            resolveColor = Color.Lerp(color, MythlandersColors.DangerRedMain, 0.75f);
        }

        spriteBatch.DrawString(_costTextFont,
            string.Format(UiResource.CombatMovementCost_Combat_LabelTemplate, _combatMovement.Cost.Amount.Current, _currentActorResolveValue.Current),
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