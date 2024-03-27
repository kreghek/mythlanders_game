using System;
using System.Collections.Generic;
using System.Linq;

using Client.Assets.CombatMovements;
using Client.Core;
using Client.Engine;

using CombatDicesTeam.Combats;
using CombatDicesTeam.Engine.Ui;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.Bestiary.Ui;

public class MonsterPerksPanel : ControlBase
{
    private readonly VerticalStackPanel _content;
    private readonly IReadOnlyList<MonsterPerk> _monsterPerks;
    private readonly Texture2D _monsterPerkIconsTexture;

    public MonsterPerksPanel(Texture2D controlTextures, Texture2D monsterPerkIconsTexture, SpriteFont perkNameFont, SpriteFont perkDescriptionFont,
        IEnumerable<MonsterPerk> monsterPerks) : base(controlTextures)
    {
        _monsterPerkIconsTexture = monsterPerkIconsTexture;

        _monsterPerks = monsterPerks.OrderBy(x => x.Sid).ToArray();

        var perkUiElements = CreatePerkUiElements(controlTextures, perkNameFont, perkDescriptionFont);

        _content = new VerticalStackPanel(controlTextures, ControlTextures.Transparent, perkUiElements);
    }

    protected override Point CalcTextureOffset()
    {
        return ControlTextures.Transparent;
    }

    protected override Color CalculateColor()
    {
        return Color.White;
    }

    protected override void DrawContent(SpriteBatch spriteBatch, Rectangle contentRect, Color contentColor)
    {
        _content.Rect = contentRect;
        _content.Draw(spriteBatch);
    }

    private static string CalcMonsterPerkDescription(MonsterPerk monsterPerk)
    {
        var perkDisplayValues = ExtractCombatMovementValues(monsterPerk);

        var perkSid = monsterPerk.Sid;
        var perkDescription =
            RenderDescriptionText(perkDisplayValues, perkSid);
        return perkDescription;
    }

    private List<ControlBase> CreatePerkUiElements(Texture2D controlTextures, SpriteFont perkNameFont,
        SpriteFont perkDescriptionFont)
    {
        var perkUiElements = new List<ControlBase>();
        foreach (var monsterPerk in _monsterPerks)
        {
            var perkNameText = new Text(controlTextures,
                ControlTextures.Transparent,
                perkNameFont,
                _ => Color.White,
                () => GameObjectHelper.GetLocalizedMonsterPerk(monsterPerk.Sid)
            );

            var perkDescriptionText = new RichText(controlTextures,
                ControlTextures.Transparent,
                perkDescriptionFont,
                _ => MythlandersColors.Description,
                () => CalcMonsterPerkDescription(monsterPerk)
            );

            var perkRightElement = new VerticalStackPanel(controlTextures, ControlTextures.Transparent,
                new ControlBase[]
                {
                    perkNameText,
                    perkDescriptionText
                });

            var iconLeftImage = new Image(_monsterPerkIconsTexture, new Rectangle(0, 0, 64, 64), controlTextures,
                ControlTextures.Transparent);

            var perkElement = new HorizontalStackPanel(controlTextures, ControlTextures.Transparent,
                new ControlBase[]
                {
                    iconLeftImage, perkRightElement
                });

            perkUiElements.Add(perkElement);
        }

        return perkUiElements;
    }

    private static IReadOnlyList<CombatMovementEffectDisplayValue> ExtractCombatMovementValues(MonsterPerk monsterPerk)
    {
        return ArraySegment<CombatMovementEffectDisplayValue>.Empty;
    }

    private static string GetValueTemplate(CombatMovementEffectDisplayValueTemplate valueType)
    {
        return valueType switch
        {
            CombatMovementEffectDisplayValueTemplate.Damage =>
                UiResource.CombatMovementEffectValueType_Damage_Template,
            CombatMovementEffectDisplayValueTemplate.DamageModifier =>
                UiResource.CombatMovementEffectValueType_DamageModifer_Template,
            CombatMovementEffectDisplayValueTemplate.TurnDuration =>
                UiResource.CombatMovementEffectValueType_TurnDuration_Template,
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
            CombatMovementEffectDisplayValueTemplate.Defence =>
                UiResource.CombatMovementEffectValueType_Defence_Template,
            _ => "<{0}> units"
        };
    }

    private static string GetValueText(CombatMovementEffectDisplayValue value)
    {
        var template = GetValueTemplate(value.Template);
        return string.Format(template, value.Value);
    }

    private static string RenderDescriptionText(IEnumerable<CombatMovementEffectDisplayValue> values,
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