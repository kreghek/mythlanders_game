using System;
using System.Collections.Generic;
using System.Linq;

using Client.Core;
using Client.Engine;

using CombatDicesTeam.Engine.Ui;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.CommandCenter.Ui;

internal sealed class CampaignLaunchHeroes : ControlBase
{
    private readonly IReadOnlyCollection<HeroState> _heroStates;

    public CampaignLaunchHeroes(IReadOnlyCollection<HeroState> heroStates) : base(UiThemeManager.UiContentStorage
        .GetControlBackgroundTexture())
    {
        _heroStates = heroStates;
    }

    protected override Point CalcTextureOffset()
    {
        return ControlTextures.PanelBlack;
    }

    protected override Color CalculateColor()
    {
        return Color.White;
    }

    protected override void DrawContent(SpriteBatch spriteBatch, Rectangle contentRect, Color contentColor)
    {
        var heroes = _heroStates.ToArray();
        for (var i = 0; i < heroes.Length; i++)
        {
            var hero = heroes[i];
            var heroNameLocalized = GameObjectHelper.GetLocalized(Enum.Parse<UnitName>(hero.ClassSid, true));
            spriteBatch.DrawString(UiThemeManager.UiContentStorage.GetMainFont(),
                heroNameLocalized,
                contentRect.Location.ToVector2() + new Vector2(0, i * 20),
                Color.Wheat);
        }
    }
}