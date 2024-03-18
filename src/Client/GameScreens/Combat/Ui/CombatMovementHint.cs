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
    private readonly ICombatMovementVisualizationProvider _combatMovementVisualizationProvider;

    private readonly VerticalStackPanel _content;
    private readonly IStatValue _currentActorResolveValue;

    public CombatMovementHint(CombatMovementInstance combatMovement, IStatValue currentActorResolveValue,
        ICombatMovementVisualizationProvider combatMovementVisualizationProvider)
    {
        _combatMovement = combatMovement;

        var nameTextFont = UiThemeManager.UiContentStorage.GetTitlesFont();
        var descriptionTextFont = UiThemeManager.UiContentStorage.GetMainFont();
        var costTextFont = UiThemeManager.UiContentStorage.GetTitlesFont();
        _currentActorResolveValue = currentActorResolveValue;
        _combatMovementVisualizationProvider = combatMovementVisualizationProvider;

        var combatMovementTraitTexts = Array.Empty<Text>();
        if (_combatMovement.SourceMovement.Metadata is not null)
        {
            combatMovementTraitTexts = ((CombatMovementMetadata)_combatMovement.SourceMovement.Metadata)
                .Traits
                .Select(x => new Text(
                    UiThemeManager.UiContentStorage.GetControlBackgroundTexture(),
                    ControlTextures.Panel,
                    descriptionTextFont,
                    _ => Color.White,
                    () => GameObjectHelper.GetLocalizedTrait(x.Sid))).ToArray();
        }

        _content = new VerticalStackPanel(UiThemeManager.UiContentStorage.GetControlBackgroundTexture(),
            ControlTextures.Transparent,
            new UiElementContentBase[]
            {
                new Text(UiThemeManager.UiContentStorage.GetControlBackgroundTexture(),
                    ControlTextures.Transparent,
                    nameTextFont,
                    _ => Color.White,
                    () => GameObjectHelper.GetLocalized(_combatMovement.SourceMovement.Sid)
                ),

                new Text(UiThemeManager.UiContentStorage.GetControlBackgroundTexture(),
                    ControlTextures.Transparent,
                    costTextFont,
                    CalcCostColor,
                    () => string.Format(
                        UiResource.CombatMovementCost_Combat_LabelTemplate,
                        _combatMovement.Cost.Amount.Current,
                        _currentActorResolveValue.Current)
                ),

                new RichText(UiThemeManager.UiContentStorage.GetControlBackgroundTexture(),
                    ControlTextures.Transparent,
                    descriptionTextFont,
                    _ => Color.Wheat,
                    () => CalcCombatMoveDescription(_combatMovement)
                ),

                new HorizontalStackPanel(UiThemeManager.UiContentStorage.GetControlBackgroundTexture(),
                    ControlTextures.Transparent,
                    combatMovementTraitTexts)
            });

        ContentSize = _content.Size.ToVector2() + new Vector2(CONTENT_MARGIN * 2);
    }

    public Vector2 ContentSize { get; }

    protected override Point CalcTextureOffset()
    {
        return Point.Zero;
    }

    protected override void DrawContent(SpriteBatch spriteBatch, Rectangle clientRect, Color contentColor)
    {
        _content.Rect = clientRect;
        _content.Draw(spriteBatch);
    }

    private string CalcCombatMoveDescription(CombatMovementInstance combatMovement)
    {
        var combatMovementDisplayValues = ExtractCombatMovementValues(combatMovement);

        var combatMovementSid = _combatMovement.SourceMovement.Sid;
        var combatMoveDescription =
            RenderDescriptionText(combatMovementDisplayValues, combatMovementSid);
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

    private IReadOnlyList<CombatMovementEffectDisplayValue> ExtractCombatMovementValues(
        CombatMovementInstance combatMovement)
    {
        return _combatMovementVisualizationProvider.ExtractCombatMovementValues(combatMovement);
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
            CombatMovementEffectDisplayValueTemplate.HitPointsDamage =>
                UiResource.CombatMovementEffectValueType_HitPointsDamage_Template,
            CombatMovementEffectDisplayValueTemplate.ResolveDamage =>
                UiResource.CombatMovementEffectValueType_ResolveDamage_Template,
            CombatMovementEffectDisplayValueTemplate.HitPoints =>
                UiResource.CombatMovementEffectValueType_HitPoints_Template,
            CombatMovementEffectDisplayValueTemplate.ShieldPoints =>
                UiResource.CombatMovementEffectValueType_ShieldPoints_Template,
            _ => "<{0}> units"
        };
    }

    private static string GetValueText(CombatMovementEffectDisplayValue value)
    {
        var template = GetValueTemplate(value.Template);
        return string.Format(template, value.Value);
    }

    private static string RenderDescriptionText(IReadOnlyList<CombatMovementEffectDisplayValue> values,
        CombatMovementSid combatMovementSid)
    {
        var descriptionMarkupText =
            StringHelper.LineBreaking(GameObjectHelper.GetLocalizedDescription(combatMovementSid), 60);

        foreach (var value in values)
        {
            var valueText = GetValueText(value);
            var styledValueText = $"<style=color1>{valueText}</style>";
            descriptionMarkupText = descriptionMarkupText.Replace($"<{value.Tag}>", styledValueText);
        }

        return descriptionMarkupText;
    }
}