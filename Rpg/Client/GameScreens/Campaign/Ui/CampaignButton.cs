using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Engine;

namespace Client.GameScreens.Campaign.Ui;

internal sealed class CampaignButton : ButtonBase
{
    private readonly Texture2D _icon;
    private readonly Rectangle? _iconRect;

    public CampaignButton(IconData iconData, string description)
    {
        _icon = iconData.Spritesheet;
        _iconRect = iconData.SourceRect;
        Description = description;
    }

    public string Description { get; }

    protected override Point CalcTextureOffset() => ControlTextures.Campaign;

    protected override void DrawContent(SpriteBatch spriteBatch, Rectangle contentRect, Color color)
    {
        spriteBatch.Draw(_icon, contentRect, _iconRect, color);
    }
}