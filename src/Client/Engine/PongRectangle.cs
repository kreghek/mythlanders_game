﻿using Microsoft.Xna.Framework;

namespace Client.Engine;

internal class PongRectangle
{
    private readonly Point _size;
    private readonly Rectangle _parentRectange;
    private readonly IPongRectangleRandomSource _randomSource;

    private Vector2 _bgCurrentPosition;

    private Vector2 _bgMoveVector;

    public PongRectangle(Point size, Rectangle parentRectange, IPongRectangleRandomSource randomSource)
    {
        _size = size;
        _parentRectange = parentRectange;
        _randomSource = randomSource;

        _bgMoveVector = randomSource.GetRandomVector();
    }

    public void Update(double timeElapsedSeconds)
    {
        var nextRect = new Rectangle((_bgCurrentPosition + _bgMoveVector).ToPoint(), _size);

        if (nextRect.Contains(_parentRectange))
        {
            _bgCurrentPosition += _bgMoveVector * (float)timeElapsedSeconds;
        }
        else
        {
            _bgMoveVector = _randomSource.GetRandomVector();
        }
    }

    public Rectangle GetRect() => new(_bgCurrentPosition.ToPoint(), _size);
}