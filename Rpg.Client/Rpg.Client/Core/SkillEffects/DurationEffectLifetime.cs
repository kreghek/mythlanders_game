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
            _duration.Decrease();

            if (_duration.IsOut)
            {
                Disposed?.Invoke(this, EventArgs.Empty);
            }
        }

        public void MergeWith(IEffectLifetime effect)
        {
            if (effect is DurationEffectLifetime durationEffectLifetime)
            {
                _duration.Increase(durationEffectLifetime._duration);
            }
        }

        public bool CanBeMerged()
        {
            return true;
        }

        private EffectDuration _duration;

        public DurationEffectLifetime(EffectDuration duration)
        {
            _duration = duration;
        }
    }
}