using System;
using System.Collections.Generic;
using System.Linq;

using Client.Assets.CombatMovements;
using Client.Core;
using Client.Engine;

using CombatDicesTeam.Combats;
using CombatDicesTeam.Engine.Ui;

using GameAssets.Combats;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.Combat.Ui;

internal class CombatMovementHint : HintBase
{
    private readonly CombatMovementInstance _combatMovement;
    private readonly IStatValue _currentActorResolveValue;
    private readonly ICombatMovementVisualizationProvider _combatMovementVisualizationProvider;
    private readonly SpriteFont _descriptionTextFont;
    private readonly SpriteFont _nameTextFont;

    private readonly VerticalStackPanel _content;

    public CombatMovementHint(CombatMovementInstance combatMovement, IStatValue currentActorResolveValue, ICombatMovementVisualizationProvider combatMovementVisualizationProvider)
    {
        _combatMovement = combatMovement;
        
        _nameTextFont = UiThemeManager.UiContentStorage.GetTitlesFont();
        _descriptionTextFont = UiThemeManager.UiContentStorage.GetMainFont();
        var costTextFont = UiThemeManager.UiContentStorage.GetTitlesFont();
        _currentActorResolveValue = currentActorResolveValue;
        _combatMovementVisualizationProvider = combatMovementVisualizationProvider;
        var combatMoveTitle = GameObjectHelper.GetLocalized(_combatMovement.SourceMovement.Sid);

        var combatMoveDescription = CalcCombatMoveDescription(combatMovement);

        ContentSize = CalcContentSize(combatMoveTitle, combatMoveDescription);

        var combatMovementTraitTexts = Array.Empty<Text>();
        if (_combatMovement.SourceMovement.Metadata is not null)
        {
            combatMovementTraitTexts = ((CombatMovementMetadata)_combatMovement.SourceMovement.Metadata)
                .Traits
                .Select(x => new Text(
                    UiThemeManager.UiContentStorage.GetControlBackgroundTexture(),
                    ControlTextures.Panel,
                    _descriptionTextFont,
                    _ => Color.White,
                    () => x.Sid)).ToArray();
        }

        _content = new VerticalStackPanel(UiThemeManager.UiContentStorage.GetControlBackgroundTexture(), ControlTextures.Transparent,
            new ControlBase[]
            {
                new Text(UiThemeManager.UiContentStorage.GetControlBackgroundTexture(),
                    ControlTextures.Transparent,
                    _nameTextFont,
                    _=>Color.White,
                    ()=> GameObjectHelper.GetLocalized(_combatMovement.SourceMovement.Sid)
                    ),
                
                new Text(UiThemeManager.UiContentStorage.GetControlBackgroundTexture(),
                    ControlTextures.Transparent,
                    costTextFont,
                    CalcCostColor,
                    ()=> string.Format(
                        UiResource.CombatMovementCost_Combat_LabelTemplate,
                        _combatMovement.Cost.Amount.Current,
                        _currentActorResolveValue.Current)
                ),
                
                new Text(UiThemeManager.UiContentStorage.GetControlBackgroundTexture(),
                    ControlTextures.Transparent,
                    _descriptionTextFont,
                    _=>Color.Wheat,
                    ()=> CalcCombatMoveDescription(_combatMovement)
                ),
                
                new HorizontalStackPanel(UiThemeManager.UiContentStorage.GetControlBackgroundTexture(),
                    ControlTextures.Transparent,
                    combatMovementTraitTexts)
            });
    }

    private string CalcCombatMoveDescription(CombatMovementInstance combatMovement)
    {
        var combatMovementDisplayValues = ExtractCombatMovementValues(combatMovement);

        var combatMovementSid = _combatMovement.SourceMovement.Sid;
        var combatMoveDescription = StringHelper.LineBreaking(
            RenderDescriptionText(combatMovementDisplayValues, combatMovementSid),
            60);
        return combatMoveDescription;
    }

    private Color CalcCostColor(Color color)
    {
        var resolveColor = Color.Lerp(color, MythlandersColors.MaxDark, 0.5f);
        if (_combatMovement.Cost.Amount.Current > _currentActorResolveValue.Current)
        {
            resolveColor = Color.Lerp(color, MythlandersColors.DangerRedMain, 0.75f);
        }

        return resolveColor;
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
        _content.Draw(spriteBatch);
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