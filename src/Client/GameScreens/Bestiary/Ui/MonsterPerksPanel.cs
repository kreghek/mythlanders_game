using System;
using System.Collections.Generic;
using System.Linq;

using Client.Core;
using Client.Engine;

using CombatDicesTeam.Engine.Ui;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.Bestiary.Ui;

public class MonsterPerksPanel: ControlBase
{
    private readonly IReadOnlyList<MonsterPerk> _monsterPerks;

    private readonly VerticalStackPanel _content;

    public MonsterPerksPanel(Texture2D controlTextures, SpriteFont perkNameFont, SpriteFont perkDescriptionFont, IEnumerable<MonsterPerk> monsterPerks) : base(controlTextures)
    {
        _monsterPerks = monsterPerks.OrderBy(x => x.Sid).ToArray();

        var perkUiElements = CreatePerkUiElements(controlTextures: controlTextures, perkNameFont: perkNameFont);

        _content = new VerticalStackPanel(controlTextures, ControlTextures.Transparent, perkUiElements);
    }

    private List<ControlBase> CreatePerkUiElements(Texture2D controlTextures, SpriteFont perkNameFont)
    {
        var perkUiElements = new List<ControlBase>();
        foreach (var monsterPerk in _monsterPerks)
        {
            var element = new Text(controlTextures,
                ControlTextures.Transparent,
                perkNameFont,
                _ => Color.White,
                () => GameObjectHelper.GetLocalized(monsterPerk.Sid)
            );

            perkUiElements.Add(element);
        }

        return perkUiElements;
    }

    protected override Point CalcTextureOffset() => ControlTextures.Transparent;

    protected override Color CalculateColor() => Color.White;

    protected override void DrawContent(SpriteBatch spriteBatch, Rectangle contentRect, Color contentColor)
    {
        _content.Rect = contentRect;
        _content.Draw(spriteBatch);
    }
}