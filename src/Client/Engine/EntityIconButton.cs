using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.Engine;

internal sealed class EntityIconButton<T> : EntityButtonBase<T>
{
    private readonly Texture2D _icon;
    private readonly Rectangle? _iconRect;

    public EntityIconButton(IconData iconData, T entity) : base(entity)
    {
        _icon = iconData.Spritesheet;
        _iconRect = iconData.SourceRect;
    }

    protected override Point CalcTextureOffset()
    {
        return Point.Zero;
    }

    protected override void DrawContent(SpriteBatch spriteBatch, Rectangle contentRect, Color color)
    {
        spriteBatch.Draw(_icon, contentRect, _iconRect, color);
    }
}