using System;

namespace Rpg.Client.Core.SkillEffects
{
    internal sealed class DurationEffectLifetime : IEffectLifetime
    {
        public event EventHandler? Disposed;

        public string GetTextDescription()
        {
            return _duration.ToString();
        }

        public void Update()
        {
            _duration--;

            if (_duration <= 0)
            {
                Disposed?.Invoke(this, EventArgs.Empty);
            }
        }

        public void MergeWith(IEffectLifetime effect)
        {
            if (effect is DurationEffectLifetime durationEffectLifetime)
            {
                _duration += durationEffectLifetime._duration;
            }
        }

        private int _duration;

        public DurationEffectLifetime(int duration)
        {
            _duration = duration;
        }
    }
}