using System;
using System.Collections.Generic;

namespace Rpg.Client.Core
{
    internal interface ICombatUnit
    {
        Unit Unit { get; }

        IReadOnlyList<CombatSkill> CombatCards { get; }
        int EnergyPool { get; set; }

        void ChangeState(CombatUnitState targetState);

        event EventHandler<UnitHitPointsChangedEventArgs>? HasTakenDamage;
        void RestoreEnergyPoint();
    }
}