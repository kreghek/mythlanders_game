using System;

using Rpg.Client.Core;
using Rpg.Client.Core.SkillEffects;

namespace Rpg.Client.Assets.SkillEffects
{
    internal sealed class HitpointThresholdEffectLifetime : IEffectLifetime
    {
        private readonly CombatUnit _boundUnit;
        private readonly float _minShare;

        public HitpointThresholdEffectLifetime(CombatUnit boundUnit, float minShare)
        {
            _boundUnit = boundUnit;
            _minShare = minShare;
            _boundUnit.HasBeenShieldPointsRestored += BoundUnit_HasBeenShieldPointsRestored;
        }

        private void BoundUnit_HasBeenShieldPointsRestored(object? sender, UnitStatChangedEventArgs e)
        {
            if (_boundUnit.HitPoints.GetShare() >= _minShare)
            {
                _boundUnit.HasBeenShieldPointsRestored -= BoundUnit_HasBeenShieldPointsRestored;
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