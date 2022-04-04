using System;
using System.Collections.Generic;

namespace Rpg.Client.Core
{
    internal interface ICombatUnit
    {
        IReadOnlyList<CombatSkill> CombatCards { get; }
        int EnergyPool { get; set; }
        Unit Unit { get; }

        void ChangeState(CombatUnitState targetState);
        void RestoreEnergyPoint();

        event EventHandler<UnitHitPointsChangedEventArgs>? HasTakenDamage;
    }
}