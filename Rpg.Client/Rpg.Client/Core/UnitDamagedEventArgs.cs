using System;

namespace Rpg.Client.Core
{
    internal sealed class UnitDamagedEventArgs : EventArgs
    {
        public UnitDamagedEventArgs(CombatUnit damageDealer)
        {
            DamageDealer = damageDealer ?? throw new ArgumentNullException(nameof(damageDealer));
        }

        public CombatUnit DamageDealer { get; }
    }
}