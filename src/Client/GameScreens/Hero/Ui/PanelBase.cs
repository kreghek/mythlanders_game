using Client;
using Client.Engine;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.Hero.Ui;

internal abstract class PanelBase : ControlBase
{
    private readonly SpriteFont _titleFont;

    protected PanelBase()
    {
        _titleFont = UiThemeManager.UiContentStorage.GetTitlesFont();
    }

    protected abstract string TitleResourceId { get; }

    public virtual void Update(GameTime gameTime) { }

    protected override Point CalcTextureOffset()
    {
        return Point.Zero;
    }

    protected override void DrawContent(SpriteBatch spriteBatch, Rectangle contentRect, Color contentColor)
    {
        var title = UiResource.ResourceManager.GetString(TitleResourceId);
        if (title is not null)
        {
            var size = _titleFont.MeasureString(title);
            spriteBatch.DrawString(_titleFont, title,
                new Vector2(contentRect.Center.X - size.X / 2, contentRect.Top - size.Y / 2), Color.Wheat);
        }

        var contentRectInner = new Rectangle(contentRect.Left + 5, contentRect.Top + 5, contentRect.Width - 5 * 2,
            contentRect.Height - 5 * 2);

        DrawPanelContent(spriteBatch, contentRectInner);
    }

    protected abstract void DrawPanelContent(SpriteBatch spriteBatch, Rectangle contentRect);
}