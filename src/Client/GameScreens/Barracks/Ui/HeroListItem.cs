using System;

using Client.Assets.Catalogs;
using Client.Core;
using Client.Engine;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.Barracks.Ui;

internal sealed class HeroListItem: ButtonBase
{
    public HeroState Hero { get; }
    private readonly SpriteFont _heroNameFont;
    private readonly Texture2D _thumbnailIcon;
    private readonly UnitName _unitName;

    public HeroListItem(HeroState hero, ICombatantGraphicsCatalog combatantGraphicsCatalog, ContentManager content, SpriteFont heroNameFont)
    {
        Hero = hero;
        _heroNameFont = heroNameFont;
        var classSid = hero.ClassSid;
        _unitName = Enum.Parse<UnitName>(classSid, false);
        var thumbnailPath = combatantGraphicsCatalog.GetGraphics(classSid).ThumbnailPath;
        _thumbnailIcon = content.Load<Texture2D>(thumbnailPath);
    }
    
    protected override Point CalcTextureOffset() => ControlTextures.Button2;

    protected override void DrawContent(SpriteBatch spriteBatch, Rectangle contentRect, Color contentColor)
    {
        spriteBatch.Draw(_thumbnailIcon, contentRect.Location.ToVector2(), Color.White);
        spriteBatch.DrawString(_heroNameFont,
            GameObjectHelper.GetLocalized(_unitName),
            contentRect.Location.ToVector2() + new Vector2(32 + CONTENT_MARGIN, 0),
            Color.Wheat);
    }
}