using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.PreHistory;

internal sealed class FractionScenePreHistoryBackground : IPreHistoryBackground
{
    private readonly Texture2D _leftDisabledTexture;
    private readonly Texture2D _leftTexture;
    private readonly Texture2D _rightDisabledTexture;
    private readonly Texture2D _rightTexture;
    private int? _hoverSelected;

    private int? _optionSelected;

    public FractionScenePreHistoryBackground(Texture2D leftTexture, Texture2D leftDisabledTexture,
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

        if (_optionSelected is not null)
        {
            if (_optionSelected == 0)
            {
                rightTexture = _rightDisabledTexture;
            }
            else if (_optionSelected == 1)
            {
                leftTexture = _leftDisabledTexture;
            }
        }
        else if (_hoverSelected is not null)
        {
            if (_hoverSelected == 0)
            {
                rightTexture = _rightDisabledTexture;
            }
            else if (_hoverSelected == 1)
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
        _optionSelected = index;
    }

    public void HoverOption(int? index)
    {
        _hoverSelected = index;
    }
}