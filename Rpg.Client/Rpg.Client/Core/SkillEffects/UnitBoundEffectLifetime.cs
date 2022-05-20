using System;

namespace Rpg.Client.Core.SkillEffects
{
    internal sealed class UnitBoundEffectLifetime : IEffectLifetime
    {
        private readonly Unit _boundUnit;

        public UnitBoundEffectLifetime(Unit boundUnit)
        {
            _boundUnit = boundUnit;
        }

        public event EventHandler? Disposed;

        public string GetTextDescription()
        {
            return string.Empty;
        }

        public void MergeWith(IEffectLifetime effect)
        {
        }

        public void Update()
        {
            if (_boundUnit.IsDead)
            {
                Disposed?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}