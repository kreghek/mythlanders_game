using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.PreHistory;

internal sealed class FractionScene : IPreHistoryScene
{
    private readonly Texture2D _leftDisabledTexture;
    private readonly Texture2D _leftTexture;
    private readonly Texture2D _rightDisabledTexture;
    private readonly Texture2D _rightTexture;
    private int? _hoverOption;

    private int? _selectedOption;

    public FractionScene(Texture2D leftTexture, Texture2D leftDisabledTexture,
        Texture2D rightTexture, Texture2D rightDisabledTexture)
    {
        _leftTexture = leftTexture;
        _leftDisabledTexture = leftDisabledTexture;
        _rightTexture = rightTexture;
        _rightDisabledTexture = rightDisabledTexture;
    }

    public void Update(GameTime gameTime, bool isInteractive)
    {
    }

    public void Draw(SpriteBatch spriteBatch, Rectangle contentRect, double transition)
    {
        var leftTexture = _leftTexture;
        var rightTexture = _rightTexture;

        if (_selectedOption is not null)
        {
            if (_selectedOption == 0)
            {
                rightTexture = _rightDisabledTexture;
            }
            else if (_selectedOption == 1)
            {
                leftTexture = _leftDisabledTexture;
            }
        }
        else if (_hoverOption is not null)
        {
            if (_hoverOption == 0)
            {
                rightTexture = _rightDisabledTexture;
            }
            else if (_hoverOption == 1)
            {
                leftTexture = _leftDisabledTexture;
            }
        }

        spriteBatch.Draw(leftTexture,
            contentRect.Location.ToVector2(),
            Color.Lerp(Color.Transparent, Color.White, (float)transition));

        spriteBatch.Draw(rightTexture,
            new Vector2(contentRect.Right - rightTexture.Width, 0),
            Color.Lerp(Color.Transparent, Color.White, (float)transition));
    }

    public void SelectOption(int index)
    {
        _selectedOption = index;
    }

    public void HoverOption(int? index)
    {
        _hoverOption = index;
    }
}