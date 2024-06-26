using System.Collections.Generic;

using Client.Core;
using Client.Engine;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.Barracks.Ui;

internal class PerkInfoPanel : PanelBase
{
    private readonly HeroState _character;
    private readonly SpriteFont _mainFont;

    public PerkInfoPanel(HeroState character)
    {
        _character = character;
        _mainFont = UiThemeManager.UiContentStorage.GetMainFont();
    }

    protected override string TitleResourceId => nameof(UiResource.HeroPerkInfoTitle);

    protected override Color CalculateColor()
    {
        return Color.White;
    }

    protected override void DrawPanelContent(SpriteBatch spriteBatch, Rectangle contentRect)
    {
        var sb = new List<string>();

        foreach (var perk in _character.Perks)
        {
            var localizedName = GameObjectHelper.GetLocalized(perk);
            sb.Add(localizedName);

            var localizedDescription = GameObjectHelper.GetLocalizedDescription(perk);
            sb.Add(localizedDescription);
        }

        for (var statIndex = 0; statIndex < sb.Count; statIndex++)
        {
            var line = sb[statIndex];
            spriteBatch.DrawString(_mainFont, line,
                new Vector2(contentRect.Left, contentRect.Top + statIndex * 22), Color.Wheat);
        }
    }
}