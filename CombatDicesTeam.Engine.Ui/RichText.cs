using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CombatDicesTeam.Engine.Ui;

public sealed class RichText : UiElementContentBase
{
    private readonly Func<Color, Color> _colorDelegate;
    private readonly SpriteFont _font;
    private readonly Func<string> _textDelegate;
    private readonly Point _textureOffset;

    public RichText(Texture2D texture, Point textureOffset, SpriteFont font, Func<Color, Color> colorDelegate,
        Func<string> textDelegate) : base(texture)
    {
        _textureOffset = textureOffset;
        _font = font;
        _colorDelegate = colorDelegate;
        _textDelegate = textDelegate;
    }

    public override Point Size =>
        (_font.MeasureString(GetPlantText(_textDelegate())) + new Vector2(CONTENT_MARGIN)).ToPoint();

    protected override Point CalcTextureOffset()
    {
        return _textureOffset;
    }

    protected override Color CalculateColor()
    {
        return Color.White;
    }

    protected override void DrawBackground(SpriteBatch spriteBatch, Color color)
    {
        // No background
    }

    protected override void DrawContent(SpriteBatch spriteBatch, Rectangle contentRect, Color contentColor)
    {
        var inputText = _textDelegate();
        var nodes = TextParser.ParseText(inputText);
        var currentPosition = contentRect.Location.ToVector2();

        foreach (var node in nodes)
        {
            foreach (var symbol in node.Value)
            {
                var symbolSize = _font.MeasureString(symbol.ToString());

                if (symbol == '\n')
                {
                    currentPosition = new Vector2(contentRect.Left, currentPosition.Y + symbolSize.Y / 2);
                }
                else
                {
                    var currentColor = _colorDelegate(contentColor);

                    if (node.Style.ColorIndex is not null)
                    {
                        currentColor = Color.Lerp(currentColor, Color.Orange, 0.75f);
                    }

                    spriteBatch.DrawString(_font, symbol.ToString(), currentPosition,
                        currentColor);
                    currentPosition += new Vector2(symbolSize.X, 0);
                }
            }
        }
    }

    private static string GetPlantText(string text)
    {
        return string.Join("", TextParser.ParseText(text).Select(x => x.Value));
    }
}