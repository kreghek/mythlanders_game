using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rpg.Client.Core
{
    internal class Attack
    {
        private readonly CombatUnit? _attackerUnit;
        private readonly CombatUnit? _targetUnit;

        public Attack(CombatUnit? attackerUnit, CombatUnit? targetUnit)
        {
            _attackerUnit = attackerUnit;
            _targetUnit = targetUnit;
        }
    }
}