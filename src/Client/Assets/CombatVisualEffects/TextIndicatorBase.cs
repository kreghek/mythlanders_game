using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.Assets.CombatVisualEffects;

internal abstract class TextIndicatorBase : ICombatVisualEffect
{
    private const float LIFETIME_SECONDS = 2;

    private readonly SpriteFont _font;
    private readonly Vector2 _targetPosition;
    private float _lifetimeCounter;
    private Vector2 _position;

    public TextIndicatorBase(Vector2 startPosition, SpriteFont font)
    {
        _position = startPosition + Vector2.UnitY * -64;
        _targetPosition = _position + Vector2.UnitY * -64;
        _font = font;
        _lifetimeCounter = LIFETIME_SECONDS;
    }

    protected abstract Color GetColor();

    protected abstract string GetText();

    public bool IsDestroyed { get; private set; }

    public void DrawBack(SpriteBatch spriteBatch)
    {
    }

    public void DrawFront(SpriteBatch spriteBatch)
    {
        var text = GetText();

        for (var x = -1; x <= 1; x++)
        {
            for (var y = -1; y <= 1; y++)
            {
                spriteBatch.DrawString(_font, text, _position + new Vector2(x, y), Color.DarkCyan);
            }
        }

        spriteBatch.DrawString(_font, text, _position, GetColor());
    }

    public void Update(GameTime gameTime)
    {
        if (_lifetimeCounter <= 0)
        {
            IsDestroyed = true;

            return;
        }

        var elapsedSec = (float)gameTime.ElapsedGameTime.TotalSeconds;
        _lifetimeCounter -= elapsedSec;

        var t = 1 - _lifetimeCounter / LIFETIME_SECONDS;

        _position = Vector2.Lerp(_position, _targetPosition, t);
    }
}