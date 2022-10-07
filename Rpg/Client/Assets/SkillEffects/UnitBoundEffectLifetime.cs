using System;

using Rpg.Client.Core;
using Rpg.Client.Core.SkillEffects;

namespace Rpg.Client.Assets.SkillEffects
{
    internal sealed class UnitBoundEffectLifetime : IEffectLifetime
    {
        private readonly ICombatUnit _boundUnit;

        public UnitBoundEffectLifetime(ICombatUnit boundUnit)
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

        public bool CanBeMerged()
        {
            return false;
        }
    }
}