using GameClient.Engine;
using GameClient.Engine.CombatVisualEffects;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoGame.Extended;
using MonoGame.Extended.Particles;
using MonoGame.Extended.TextureAtlases;

namespace Client.Assets.CombatVisualEffects;

internal sealed class GunFlashCombatVisualEffect : ICombatVisualEffect
{
    private double _lifetimeCounter;
    private readonly Duration _duration = new Duration(0.15);
    private readonly Vector2 _position;
    private readonly int _size;
    private readonly TextureRegion2D _flashTexture;

    public GunFlashCombatVisualEffect(Vector2 position, int size, TextureRegion2D whiteflashTexture)
    {
        _position = position;
        _size = size;
        _flashTexture = whiteflashTexture;
    }

    public bool IsDestroyed { get; private set; }

    public void DrawBack(SpriteBatch spriteBatch)
    {
        var flashPosition = _position - new Vector2(_size / 2, _size / 2);
        if (_lifetimeCounter >= _duration.Seconds / 2)
        {
            spriteBatch.Draw(_flashTexture, new Rectangle(flashPosition.ToPoint(), new Point(_size, _size)), Color.White);
        }
        else
        {
            spriteBatch.Draw(_flashTexture, new Rectangle(flashPosition.ToPoint(), new Point(_size, _size)), Color.Black);
        }
    }

    public void DrawFront(SpriteBatch spriteBatch)
    {
        // Do nothing
    }

    public void Update(GameTime gameTime)
    {
        if (IsDestroyed)
        {
            return;
        }

        if (_lifetimeCounter >= _duration.Seconds)
        {
            IsDestroyed = true;
        }
        else
        {
            _lifetimeCounter += gameTime.GetElapsedSeconds();
        }
    }
}
