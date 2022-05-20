using System;

namespace Rpg.Client.Core.SkillEffects
{
    internal sealed class UnitBoundEffectLifetime : IEffectLifetime
    {
        private readonly Unit _boundUnit;

        public UnitBoundEffectLifetime(Unit boundUnit)
        {
            _boundUnit = boundUnit;
            _boundUnit.Dead += Unit_Dead;
        }

        private void Unit_Dead(object? sender, UnitDamagedEventArgs e)
        {
            _boundUnit.Dead -= Unit_Dead;
            Disposed?.Invoke(this, EventArgs.Empty);
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
            
        }
    }
}