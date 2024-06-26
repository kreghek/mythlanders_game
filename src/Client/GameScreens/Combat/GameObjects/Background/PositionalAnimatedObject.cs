﻿using Client.Engine;

using GameClient.Engine.Animations;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.Combat.GameObjects.Background;

internal sealed class PositionalAnimatedObject : IBackgroundObject
{
    private readonly IAnimationFrameSet _animationFrameSet;
    private readonly Sprite _sprite;

    public PositionalAnimatedObject(Texture2D texture, IAnimationFrameSet animationFrameSet, Vector2 position,
        Vector2? origin = null)
    {
        _sprite = new Sprite(texture)
        {
            Position = position,
            Origin = origin ?? Vector2.One * 0.5f
        };
        _animationFrameSet = animationFrameSet;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        var sourceRect = _animationFrameSet.GetFrameRect();

        _sprite.SourceRectangle = sourceRect;
        _sprite.Draw(spriteBatch);
    }

    public void Update(GameTime gameTime)
    {
        _animationFrameSet.Update(gameTime);
    }
}