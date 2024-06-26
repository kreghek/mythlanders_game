using System;
using System.Collections.Generic;

using Client.Core;
using Client.Engine;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.Barracks.Ui;

internal class StatsInfoPanel : PanelBase
{
    private readonly HeroState _hero;
    private readonly SpriteFont _mainFont;

    public StatsInfoPanel(HeroState hero)
    {
        _hero = hero;
        _mainFont = UiThemeManager.UiContentStorage.GetMainFont();
    }

    protected override string TitleResourceId => nameof(UiResource.HeroGeneralInfoTitle);

    protected override Color CalculateColor()
    {
        return Color.White;
    }

    protected override void DrawPanelContent(SpriteBatch spriteBatch, Rectangle contentRect)
    {
        var unitName = Enum.Parse<UnitName>(_hero.ClassSid, true);
        var name = GameObjectHelper.GetLocalized(unitName);

        var sb = new List<string>
        {
            name,
            string.Format(UiResource.HitPointsLabelTemplate, _hero.HitPoints.Current)
            // string.Format(UiResource.ShieldPointsLabelTemplate,
            //     _hero.Stats.Single(x => x.Type == UnitStatType.ShieldPoints).Value.ActualMax),
            // string.Format(UiResource.ManaLabelTemplate, 0,
            //     _hero.EnergyPoolSize),
            // string.Format(UiResource.CombatLevelTemplate, _hero.Level),
            // string.Format(UiResource.CombatLevelUpTemplate, _hero.LevelUpXpAmount)
        };

        for (var statIndex = 0; statIndex < sb.Count; statIndex++)
        {
            var line = sb[statIndex];
            spriteBatch.DrawString(_mainFont, line,
                new Vector2(contentRect.Left, contentRect.Top + statIndex * 22), Color.Wheat);
        }
    }
}