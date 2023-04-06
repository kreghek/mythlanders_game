using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Engine;

namespace Client.GameScreens.Combat.Ui;

internal sealed class FieldManeuverIndicatorPanel : ControlBase
{
    private readonly IManeuverContext _context;
    private readonly SpriteFont _font;

    public FieldManeuverIndicatorPanel(SpriteFont font, IManeuverContext context)
    {
        _font = font;
        _context = context;
    }

    protected override Point CalcTextureOffset()
    {
        return ControlTextures.PanelBlack;
    }

    protected override Color CalculateColor()
    {
        return Color.Lerp(Color.White, Color.Transparent, 0.5f);
    }

    public bool IsHidden => _context.ManeuversAvailable.GetValueOrDefault() <= 0;

    protected override void DrawContent(SpriteBatch spriteBatch, Rectangle contentRect, Color contentColor)
    {
        if (_context.ManeuversAvailable.GetValueOrDefault() > 0)
        {
            var text = UiResource.AvailableManeuversIndicatorTemplate;
            if (_context.ManeuversAvailable > 1)
            {
                text = " x" + _context.ManeuversAvailable;
            }

            var textSize = _font.MeasureString(text);
            spriteBatch.DrawString(_font, text,
                new Vector2(contentRect.Location.X + (contentRect.Width - textSize.X) / 2, contentRect.Y), Color.Cyan);
        }
    }
}