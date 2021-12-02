using System;

namespace Rpg.Client.Core
{
    internal sealed class UnitDamagedEventArgs : EventArgs
    {
        public UnitDamagedEventArgs(ICombatUnit damageDealer)
        {
            DamageDealer = damageDealer ?? throw new ArgumentNullException(nameof(damageDealer));
        }

        public ICombatUnit DamageDealer { get; }
    }
}