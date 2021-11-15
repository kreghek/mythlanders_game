using System;

using Microsoft.Xna.Framework;

namespace Rpg.Client.Models.Combat
{
    internal sealed class ScreenShaker
    {
        private const double ITERATION_DURATION = 0.015;
        
        private double _counter;
        private bool _isEnabled;
        private double? _targetDuration;
        private double _currentIterationCounter;
        private Vector2? _offset;
        private static readonly Random _random = new Random();
        private float _amplitude;
        
        public void Start(double? seconds)
        {
            _isEnabled = true;
            _targetDuration = seconds;
            _amplitude = 20;
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
                    _amplitude *= 0.9f;
                }
            }
            else
            {
                _offset = null;
            }
        }

        public Vector2? GetOffset()
        {
            if (_offset is null)
            {
                return null;
            }

            return _offset.Value * _amplitude;
        }
    }
}