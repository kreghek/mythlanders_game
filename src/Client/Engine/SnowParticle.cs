﻿using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.Engine;

internal class SnowParticle : IParticle
{
    private readonly Rectangle _sourceRect;
    private readonly Vector2 _startPosition;
    private readonly int _startTTL;
    private readonly Vector2 _targetPosition;
    private readonly float _xAmpl;

    public SnowParticle(Texture2D texture, Rectangle sourceRect, Vector2 position, Vector2 targetPosition,
        Vector2 velocity,
        float angle, float angularVelocity, Color color, float size, int ttl, float xAmpl)
    {
        _sourceRect = sourceRect;
        _targetPosition = targetPosition;
        Texture = texture;
        Position = position;
        Velocity = velocity;
        Angle = angle;
        AngularVelocity = angularVelocity;
        Color = color;
        Size = size;
        TTL = ttl;

        _startTTL = ttl;
        _startPosition = position;

        _xAmpl = xAmpl;
    }

    public float Angle { get; set; } // The current angle of rotation of the particle
    public float AngularVelocity { get; set; } // The speed that the angle is changing
    public Color Color { get; set; } // The color of the particle
    public Vector2 Position { get; set; } // The current position of the particle        
    public float Size { get; set; } // The size of the particle
    public Texture2D Texture { get; set; } // The texture that will be drawn to represent the particle
    public int TTL { get; set; } // The 'time to live' of the particle
    public Vector2 Velocity { get; set; } // The speed of the particle at the current instance

    public void Draw(SpriteBatch spriteBatch)
    {
        var origin = new Vector2(Texture.Width * 0.5f, Texture.Height * 0.5f);

        spriteBatch.Draw(Texture, Position, _sourceRect, Color,
            Angle, origin, Size, SpriteEffects.None, 0f);
    }

    public void Update()
    {
        TTL--;

        var t = 1 - (float)TTL / _startTTL;

        var upVector = Vector2.UnitY * (float)Math.Sin(t * Math.PI * 2) * 32;
        var xVector = Vector2.UnitX * (float)Math.Sin(t * Math.PI * 2 * 4) * _xAmpl;
        Position = Vector2.Lerp(_startPosition, _targetPosition, t) + upVector + xVector;
    }
}