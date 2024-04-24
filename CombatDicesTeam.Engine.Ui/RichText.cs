using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CombatDicesTeam.Engine.Ui;

public sealed class RichText : ControlBase
{
    private readonly Func<Color, Color> _colorDelegate;
    private readonly SpriteFont _font;
    private readonly Func<string> _textDelegate;
    private readonly Point _textureOffset;

    private readonly Color _highlightColor1 = new(109, 234, 214);
    private readonly Color _highlightColor2 = new(243, 168, 51);
    private readonly Color _highlightColor3 = new(243, 168, 51);

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
        var currentSymbolPosition = contentRect.Location.ToVector2();

        foreach (var node in nodes)
        {
            foreach (var symbol in node.Value)
            {
                var symbolSize = _font.MeasureString(symbol.ToString());

                if (symbol == '\n')
                {
                    currentSymbolPosition = new Vector2(contentRect.Left, currentSymbolPosition.Y + symbolSize.Y / 2);
                }
                else
                {
                    var symbolColor = GetTextColorByIndex(node.Style.ColorIndex) ?? _colorDelegate(contentColor);

                    spriteBatch.DrawString(_font, symbol.ToString(), currentSymbolPosition,
                        symbolColor);
                    currentSymbolPosition += new Vector2(symbolSize.X, 0);
                }
            }
        }
    }

    private static string GetPlantText(string text)
    {
        return string.Join("", TextParser.ParseText(text).Select(x => x.Value));
    }

    private Color? GetTextColorByIndex(int? colorIndex)
    {
        return colorIndex switch
        {
            1 => _highlightColor1,
            2 => _highlightColor2,
            3 => _highlightColor3,
            _ => null
        };
    }
}