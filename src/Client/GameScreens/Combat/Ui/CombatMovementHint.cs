using System;
using System.Collections.Generic;
using System.Linq;

using Client.Assets;
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
        var costTextFont = UiThemeManager.UiContentStorage.GetMainFont();
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
                    () => $"[{GameObjectHelper.GetLocalizedTrait(x.Sid)}]")).ToArray();
        }

        var descriptionElements = new List<ControlBase>
        {
            new Text(UiThemeManager.UiContentStorage.GetControlBackgroundTexture(),
                ControlTextures.Transparent,
                nameTextFont,
                _ => Color.White,
                () => GameObjectHelper.GetLocalized(_combatMovement.SourceMovement.Sid)
            ),

            new RichText(UiThemeManager.UiContentStorage.GetControlBackgroundTexture(),
                ControlTextures.Transparent,
                descriptionTextFont,
                _ => new Color(232, 210, 130),
                () => CalcCombatMoveDescription(_combatMovement)
            )
        };

        if (_combatMovement.Cost.Amount.Current > 0)
        {
            descriptionElements.Add(new Text(UiThemeManager.UiContentStorage.GetControlBackgroundTexture(),
                ControlTextures.Transparent,
                costTextFont,
                CalcCostColor,
                () => string.Format(
                    UiResource.CombatMovementCost_Combat_LabelTemplate,
                    _combatMovement.Cost.Amount.Current,
                    _currentActorResolveValue.Current)
            ));
        }

        descriptionElements.Add(new HorizontalStackPanel(UiThemeManager.UiContentStorage.GetControlBackgroundTexture(),
            ControlTextures.Transparent,
            combatMovementTraitTexts));

        _content = new VerticalStackPanel(UiThemeManager.UiContentStorage.GetControlBackgroundTexture(),
            ControlTextures.Transparent,
            descriptionElements);

        ContentSize = _content.Size.ToVector2() + new Vector2(CONTENT_MARGIN * 2);
    }

    public Vector2 ContentSize { get; }

    protected override Point CalcTextureOffset()
    {
        return new Point(0, 96);
    }

    protected override Color CalculateColor()
    {
        return Color.Lerp(Color.Transparent, Color.White, 0.85f);
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
        var resolveColor = Color.Lerp(color, new Color(232, 210, 130), 0.5f);
        if (_combatMovement.Cost.Amount.Current > _currentActorResolveValue.Current)
        {
            resolveColor = Color.Lerp(color, MythlandersColors.DangerRedMain, 0.75f);
        }

        return resolveColor;
    }

    private IReadOnlyList<DescriptionKeyValue> ExtractCombatMovementValues(
        CombatMovementInstance combatMovement)
    {
        return _combatMovementVisualizationProvider.ExtractCombatMovementValues(combatMovement);
    }

    private static string GetValueTemplate(DescriptionKeyValueTemplate valueType)
    {
        return valueType switch
        {
            DescriptionKeyValueTemplate.Damage =>
                UiResource.CombatMovementEffectValueType_Damage_Template,
            DescriptionKeyValueTemplate.DamageModifier =>
                UiResource.CombatMovementEffectValueType_DamageModifer_Template,
            DescriptionKeyValueTemplate.TurnDuration =>
                UiResource.CombatMovementEffectValueType_TurnDuration_Template,
            DescriptionKeyValueTemplate.RoundDuration =>
                UiResource.CombatMovementEffectValueType_RoundDuration_Template,
            DescriptionKeyValueTemplate.HitPointsDamage =>
                UiResource.CombatMovementEffectValueType_HitPointsDamage_Template,
            DescriptionKeyValueTemplate.ResolveDamage =>
                UiResource.CombatMovementEffectValueType_ResolveDamage_Template,
            DescriptionKeyValueTemplate.HitPoints =>
                UiResource.CombatMovementEffectValueType_HitPoints_Template,
            DescriptionKeyValueTemplate.ShieldPoints =>
                UiResource.CombatMovementEffectValueType_ShieldPoints_Template,
            DescriptionKeyValueTemplate.Defence =>
                UiResource.CombatMovementEffectValueType_Defence_Template,
            DescriptionKeyValueTemplate.Resolve =>
                UiResource.CombatMovementEffectValueType_Resolve_Template,
            _ => "<{0}> units"
        };
    }

    private static string GetValueText(DescriptionKeyValue value)
    {
        var template = GetValueTemplate(value.Template);
        return string.Format(template, value.Value);
    }

    private static string RenderDescriptionText(IReadOnlyList<DescriptionKeyValue> values,
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