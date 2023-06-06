using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.Engine;

internal class IndicatorTextButton : ButtonBase
{
    private readonly SpriteFont _font;
    private readonly Texture2D _indicatorsTexture;
    private readonly string _resourceSid;

    private float _counter;

    public IndicatorTextButton(string resourceSid, Texture2D indicatorsTexture)
    {
        _font = UiThemeManager.UiContentStorage.GetMainFont();
        _resourceSid = resourceSid;
        _indicatorsTexture = indicatorsTexture;
    }

    public Func<bool>? IndicatingSelector { get; set; }

    protected override Point CalcTextureOffset()
    {
        return Point.Zero;
    }

    protected override void DrawBackground(SpriteBatch spriteBatch, Color color)
    {
        if (IndicatingSelector is not null && IndicatingSelector())
        {
            _counter += 0.05f;
            if (_counter > 1)
            {
                _counter = 0;
            }

            var totalColor = Color.Lerp(color, Color.LimeGreen, _counter);

            base.DrawBackground(spriteBatch, totalColor);
        }
        else
        {
            base.DrawBackground(spriteBatch, color);
        }
    }

    protected override void DrawContent(SpriteBatch spriteBatch, Rectangle contentRect, Color contentColor)
    {
        if (IndicatingSelector is not null && IndicatingSelector())
        {
            const int INDICATOR_FRAME_COUNT = 2;
            var index = (int)Math.Round(INDICATOR_FRAME_COUNT * _counter, MidpointRounding.AwayFromZero);
            const int INDICATOR_SIZE = 16;
            var sourceRect = new Rectangle(INDICATOR_SIZE * index, 0, INDICATOR_SIZE, INDICATOR_SIZE);
            spriteBatch.Draw(_indicatorsTexture,
                new Rectangle(contentRect.Right - INDICATOR_SIZE, contentRect.Top, INDICATOR_SIZE, INDICATOR_SIZE),
                sourceRect, contentColor);
        }

        var localizedTitle = UiResource.ResourceManager.GetString(_resourceSid) ?? _resourceSid;

        var (textWidth, textHeight) = _font.MeasureString(localizedTitle);
        var widthDiff = contentRect.Width - textWidth;
        var heightDiff = contentRect.Height - textHeight;
        var textPosition = new Vector2(
            widthDiff / 2 + contentRect.Left,
            heightDiff / 2 + contentRect.Top);

        spriteBatch.DrawString(_font, localizedTitle, textPosition, Color.SaddleBrown);
    }
}