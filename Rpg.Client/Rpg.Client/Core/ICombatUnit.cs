using System;
using System.Collections.Generic;

namespace Rpg.Client.Core
{
    internal interface ICombatUnit
    {
        IReadOnlyList<CombatSkill> CombatCards { get; }
        int EnergyPool { get; set; }
        bool IsDead { get; }

        IReadOnlyCollection<IUnitStat> Stats { get; }
        Unit Unit { get; }

        void ChangeState(CombatUnitState targetState);
        void RestoreEnergyPoint();
        void RestoreHitPoints(int heal);
        void RestoreShields();
        DamageResult TakeDamage(ICombatUnit damageDealer, int damageSource);

        event EventHandler<UnitStatChangedEventArgs>? HasTakenHitPointsDamage;
        event EventHandler<UnitDamagedEventArgs>? Dead;
    }
}