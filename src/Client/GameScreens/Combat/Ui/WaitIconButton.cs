using Client.Engine;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.Combat.Ui;

internal sealed class WaitIconButton : ButtonBase
{
    private readonly Texture2D _icon;
    private readonly Rectangle? _iconRect;

    public WaitIconButton(IconData iconData)
    {
        _icon = iconData.Spritesheet;
        _iconRect = iconData.SourceRect;
    }

    protected override Point CalcTextureOffset()
    {
        return ControlTextures.Button2;
    }

    protected override void DrawContent(SpriteBatch spriteBatch, Rectangle contentRect, Color color)
    {
        spriteBatch.Draw(_icon, contentRect, _iconRect, color);
    }
}