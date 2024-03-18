using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CombatDicesTeam.Engine.Ui;

public interface IUiElementSizeProvider
{
    Point GetSize();
}

public static class

public interface IUiElementPositionProvider
{
    Vector2 GetPosition();
}

public sealed record UiElementParent(IUiElementSizeProvider SizeProvider, IUiElementPositionProvider PositionProvider);

public sealed class UiElement
{
    private readonly IEnumerable<IUiElementContent> _content;

    public UiElement(IEnumerable<IUiElementContent> content)
    {
        _content = content;
    }
    
    public void Draw(SpriteBatch spriteBatch, UiElementParent parent, Color color)
    {
        foreach (var elementContent in _content)
        {
            elementContent.Draw(spriteBatch, parent, color);
        }
    }

    public void Update(GameTime gameTime, UiElementParent parent, IScreenProjection screenProjection)
    {
        foreach (var elementContent in _content)
        {
            elementContent.Update(gameTime, parent, screenProjection);
        }
    }
}

public interface IUiElementContent
{
    IUiElementSizeProvider SizeProvider { get; }
    IUiElementPositionProvider PositionProvider { get; }
    void Draw(SpriteBatch spriteBatch, UiElementParent parent, Color color);
    void Update(GameTime gameTime, UiElementParent parent, IScreenProjection screenProjection);
}

public abstract class UiElementContentBase1 : IUiElementContent
{
    protected UiElementContentBase1(IUiElementSizeProvider sizeProvider, IUiElementPositionProvider positionProvider)
    {
        SizeProvider = sizeProvider;
        PositionProvider = positionProvider;
    }

    public IUiElementSizeProvider SizeProvider { get; }
    public IUiElementPositionProvider PositionProvider { get; }
    public abstract void Draw(SpriteBatch spriteBatch, UiElementParent parent, Color color);
    public virtual void Update(GameTime gameTime, UiElementParent parent, IScreenProjection screenProjection) { }
}

public sealed class NineTiledImageUiElementContent : UiElementContentBase1
{
    private readonly Texture2D _texture;
    private readonly Rectangle _sourceRectangle;
    public const int CONTENT_MARGIN = 4;
    private const int CORNER_SIZE = 15;
    private const int INNER_SIZE = (16 - CORNER_SIZE) * 2;

    private static readonly Rectangle[,] _sourceRects =
    {
        {
            new(0, 0, CORNER_SIZE, CORNER_SIZE),
            new(CORNER_SIZE, 0, INNER_SIZE, CORNER_SIZE),
            new(CORNER_SIZE + INNER_SIZE, 0, CORNER_SIZE, CORNER_SIZE)
        },

        {
            new(0, CORNER_SIZE, CORNER_SIZE, INNER_SIZE),
            new(CORNER_SIZE, CORNER_SIZE, INNER_SIZE, INNER_SIZE),
            new(INNER_SIZE + CORNER_SIZE, CORNER_SIZE, CORNER_SIZE, INNER_SIZE)
        },

        {
            new(0, INNER_SIZE + CORNER_SIZE, CORNER_SIZE, CORNER_SIZE),
            new(CORNER_SIZE, INNER_SIZE + CORNER_SIZE, INNER_SIZE, CORNER_SIZE),
            new(INNER_SIZE + CORNER_SIZE, INNER_SIZE + CORNER_SIZE, CORNER_SIZE, CORNER_SIZE)
        }
    };

    public override void Draw(SpriteBatch spriteBatch, UiElementParent parent, Color color)
    {
        var rectWidth = parent.SizeProvider.GetSize().X - CORNER_SIZE * 2;
        var rectHeight = Rect.Height - CORNER_SIZE * 2;

        var destRectsBase = new[,]
        {
            {
                new Rectangle(0, 0, CORNER_SIZE, CORNER_SIZE),
                new Rectangle(CORNER_SIZE, 0, rectWidth, CORNER_SIZE),
                new Rectangle(rectWidth + CORNER_SIZE, 0, CORNER_SIZE, CORNER_SIZE)
            },
            {
                new Rectangle(0, CORNER_SIZE, CORNER_SIZE, rectHeight),
                new Rectangle(CORNER_SIZE, CORNER_SIZE, rectWidth, rectHeight),
                new Rectangle(rectWidth + CORNER_SIZE, CORNER_SIZE, CORNER_SIZE, rectHeight)
            },
            {
                new Rectangle(0, rectHeight + CORNER_SIZE, CORNER_SIZE, CORNER_SIZE),
                new Rectangle(CORNER_SIZE, rectHeight + CORNER_SIZE, rectWidth, CORNER_SIZE),
                new Rectangle(rectWidth + CORNER_SIZE, rectHeight + CORNER_SIZE, CORNER_SIZE, CORNER_SIZE)
            }
        };

        for (var x = 0; x < 3; x++)
        {
            for (var y = 0; y < 3; y++)
            {
                var sourceRect = _sourceRects[x, y];
                sourceRect.Offset(CalcTextureOffset());

                var controlPosition = destRectsBase[x, y].Location + Rect.Location;
                var destRect = new Rectangle(controlPosition, destRectsBase[x, y].Size);
                spriteBatch.Draw(_texture, destRect, sourceRect, color);
            }
        }
    }

    public NineTiledImageUiElementContent(IUiElementSizeProvider sizeProvider,
        IUiElementPositionProvider positionProvider, Texture2D texture, Rectangle sourceRectangle) : base(sizeProvider,
        positionProvider)
    {
        _texture = texture;
        _sourceRectangle = sourceRectangle;
    }
}

public abstract class UiElementContentBase
{
    public const int CONTENT_MARGIN = 4;
    private const int CORNER_SIZE = 15;
    private const int INNER_SIZE = (16 - CORNER_SIZE) * 2;

    private static readonly Rectangle[,] _sourceRects =
    {
        {
            new(0, 0, CORNER_SIZE, CORNER_SIZE),
            new(CORNER_SIZE, 0, INNER_SIZE, CORNER_SIZE),
            new(CORNER_SIZE + INNER_SIZE, 0, CORNER_SIZE, CORNER_SIZE)
        },

        {
            new(0, CORNER_SIZE, CORNER_SIZE, INNER_SIZE),
            new(CORNER_SIZE, CORNER_SIZE, INNER_SIZE, INNER_SIZE),
            new(INNER_SIZE + CORNER_SIZE, CORNER_SIZE, CORNER_SIZE, INNER_SIZE)
        },

        {
            new(0, INNER_SIZE + CORNER_SIZE, CORNER_SIZE, CORNER_SIZE),
            new(CORNER_SIZE, INNER_SIZE + CORNER_SIZE, INNER_SIZE, CORNER_SIZE),
            new(INNER_SIZE + CORNER_SIZE, INNER_SIZE + CORNER_SIZE, CORNER_SIZE, CORNER_SIZE)
        }
    };

    private readonly Texture2D _texture;

    protected UiElementContentBase(Texture2D texture)
    {
        _texture = texture;
    }

    public Rectangle Rect { get; set; }

    public virtual Point Size { get; }

    protected virtual int Margin => CONTENT_MARGIN;

    public void Draw(SpriteBatch spriteBatch)
    {
        var color = CalculateColor();

        DrawBackground(spriteBatch: spriteBatch, color: color);

        var contentRect = new Rectangle(
            Margin + Rect.Left,
            Margin + Rect.Top,
            Rect.Width - Margin * 2,
            Rect.Height - Margin * 2);

        DrawContent(spriteBatch, contentRect, color);
    }

    protected abstract Point CalcTextureOffset();

    protected abstract Color CalculateColor();

    protected virtual void DrawBackground(SpriteBatch spriteBatch, Color color)
    {
        var rectWidth = Rect.Width - CORNER_SIZE * 2;
        var rectHeight = Rect.Height - CORNER_SIZE * 2;

        var destRectsBase = new[,]
        {
            {
                new Rectangle(0, 0, CORNER_SIZE, CORNER_SIZE),
                new Rectangle(CORNER_SIZE, 0, rectWidth, CORNER_SIZE),
                new Rectangle(rectWidth + CORNER_SIZE, 0, CORNER_SIZE, CORNER_SIZE)
            },
            {
                new Rectangle(0, CORNER_SIZE, CORNER_SIZE, rectHeight),
                new Rectangle(CORNER_SIZE, CORNER_SIZE, rectWidth, rectHeight),
                new Rectangle(rectWidth + CORNER_SIZE, CORNER_SIZE, CORNER_SIZE, rectHeight)
            },
            {
                new Rectangle(0, rectHeight + CORNER_SIZE, CORNER_SIZE, CORNER_SIZE),
                new Rectangle(CORNER_SIZE, rectHeight + CORNER_SIZE, rectWidth, CORNER_SIZE),
                new Rectangle(rectWidth + CORNER_SIZE, rectHeight + CORNER_SIZE, CORNER_SIZE, CORNER_SIZE)
            }
        };

        for (var x = 0; x < 3; x++)
        {
            for (var y = 0; y < 3; y++)
            {
                var sourceRect = _sourceRects[x, y];
                sourceRect.Offset(CalcTextureOffset());

                var controlPosition = destRectsBase[x, y].Location + Rect.Location;
                var destRect = new Rectangle(controlPosition, destRectsBase[x, y].Size);
                spriteBatch.Draw(_texture, destRect, sourceRect, color);
            }
        }
    }

    protected abstract void DrawContent(SpriteBatch spriteBatch, Rectangle contentRect, Color contentColor);
}