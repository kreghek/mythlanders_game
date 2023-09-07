using Client.Engine;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.Combat.GameObjects.Background;

internal sealed class PositionalStaticObject : IBackgroundObject
{
    private readonly Sprite _sprite;

    public PositionalStaticObject(Texture2D texture, Vector2 position, Rectangle sourceRectangle, Vector2 origin)
    {
        _sprite = new Sprite(texture)
        {
            Position = position,
            SourceRectangle = sourceRectangle,
            Origin = origin
        };
    }

    public Rectangle SourceRectangle { get; }

    public void Draw(SpriteBatch spriteBatch)
    {
        _sprite.Draw(spriteBatch);
    }

    public void Update(GameTime gameTime)
    {
        // Static objects does not updates.
    }
}