using System;

using Rpg.Client.Core;
using Rpg.Client.Core.SkillEffects;

namespace Rpg.Client.Assets.SkillEffects
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

        public bool CanBeMerged()
        {
            return false;
        }
    }

    internal sealed class HitpointThresholdEffectLifetime : IEffectLifetime
    {
        private readonly Unit _boundUnit;
        private readonly float _minShare;

        public HitpointThresholdEffectLifetime(Unit boundUnit, float minShare)
        {
            _boundUnit = boundUnit;
            _minShare = minShare;
            _boundUnit.HasBeenHitPointsRestored += Unit_HasBeenHitPointsRestored;
        }

        private void Unit_HasBeenHitPointsRestored(object? sender, int e)
        {
            if (_boundUnit.HitPoints.Share >= _minShare)
            {
                _boundUnit.HasBeenHitPointsRestored -= Unit_HasBeenHitPointsRestored;
                Disposed?.Invoke(this, EventArgs.Empty);
            }
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