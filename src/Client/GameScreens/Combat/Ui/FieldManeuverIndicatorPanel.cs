using System;

using Client.Engine;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.Combat.Ui;

internal sealed class FieldManeuverIndicatorPanel : ControlBase
{
    private readonly IManeuverContext _context;
    private readonly SpriteFont _font;
    private double _counter;

    public FieldManeuverIndicatorPanel(SpriteFont font, IManeuverContext context)
    {
        _font = font;
        _context = context;
    }

    public bool IsHidden => _context.ManeuversAvailableCount.GetValueOrDefault() <= 0;

    protected override Point CalcTextureOffset()
    {
        return ControlTextures.PanelBlack;
    }

    protected override Color CalculateColor()
    {
        return Color.Lerp(Color.White, Color.Transparent, 0.5f + GetCurrentT() * 0.25f);
    }

    protected override void DrawContent(SpriteBatch spriteBatch, Rectangle contentRect, Color contentColor)
    {
        if (_context.ManeuversAvailableCount.GetValueOrDefault() <= 0)
        {
            return;
        }

        var text = UiResource.AvailableManeuversIndicatorTemplate;
        if (_context.ManeuversAvailableCount > 1)
        {
            text = " x" + _context.ManeuversAvailableCount;
        }

        DrawTextInCenterBottom(spriteBatch, contentRect, text);
    }

    private void DrawTextInCenterBottom(SpriteBatch spriteBatch, Rectangle targetRect, string text)
    {
        var textSize = _font.MeasureString(text);
        var targetRectWidth = targetRect.Width - textSize.X;
        spriteBatch.DrawString(_font, text,
            new Vector2(targetRect.Location.X + targetRectWidth / 2, targetRect.Y),
            Color.Lerp(TestamentColors.MainSciFi, Color.Transparent, 0.5f + (1 - GetCurrentT()) * 0.25f));
    }

    public void Update(GameTime gameTime)
    {
        _counter += gameTime.ElapsedGameTime.TotalSeconds;
    }

    private float GetCurrentT()
    {
        return (float)Math.Sin(_counter * 0.5f);
    }
}