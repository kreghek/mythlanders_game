using System;

using Microsoft.Xna.Framework;

namespace Client.GameScreens.Combat;

internal enum ShakeDirection
{
    FadeIn,
    FadeOut
}

internal sealed class ScreenShaker
{
    private const double ITERATION_DURATION = 0.015;
    private const int MAX_AMPLITUDE = 20;
    private static readonly Random _random = new Random();
    private float _amplitude;

    private double _counter;
    private double _currentIterationCounter;
    private bool _isEnabled;
    private Vector2? _offset;
    private ShakeDirection _shakeDirection;
    private double? _targetDuration;

    public Vector2? GetOffset()
    {
        if (_offset is null)
        {
            return null;
        }

        return _offset.Value * _amplitude;
    }


    public void Start(double? seconds, ShakeDirection shakeDirection)
    {
        _isEnabled = true;
        _targetDuration = seconds;
        _amplitude = MAX_AMPLITUDE;
        _shakeDirection = shakeDirection;
    }

    public void Stop()
    {
        _isEnabled = false;
        _counter = 0;
        _targetDuration = null;
        _offset = null;
    }

    public void Update(GameTime gameTime)
    {
        if (_isEnabled)
        {
            _counter += gameTime.ElapsedGameTime.TotalSeconds;
            if (_targetDuration is not null && _counter >= _targetDuration)
            {
                Stop();
                return;
            }

            _currentIterationCounter += gameTime.ElapsedGameTime.TotalSeconds;
            if (_currentIterationCounter >= ITERATION_DURATION)
            {
                _currentIterationCounter = 0;
                _offset = new Vector2((float)_random.NextDouble() - 0.5f, (float)_random.NextDouble() - 0.5f);
            }

            if (_amplitude > 0.00005)
            {
                var t = _counter / _targetDuration.GetValueOrDefault(1);
                var t2 = t > 1 ? 1 : (float)t;
                var t3 = _shakeDirection == ShakeDirection.FadeIn ? 1 - t2 : t2;
                _amplitude = MAX_AMPLITUDE * t3;
            }
        }
        else
        {
            _offset = null;
        }
    }
}