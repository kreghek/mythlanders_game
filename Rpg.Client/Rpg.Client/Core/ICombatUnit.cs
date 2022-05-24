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
        void RestoreHitPoints(int heal);
        void RestoreShields();
        DamageResult TakeDamage(ICombatUnit damageDealer, int damageSource);

        event EventHandler<UnitStatChangedEventArgs>? HasTakenHitPointsDamage;

        IReadOnlyCollection<IUnitStat> Stats { get; }
        bool IsDead { get; }
    }
}