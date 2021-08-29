using System;
using System.Collections.Generic;
using System.Linq;

namespace Rpg.Client.Core
{
    internal class ActiveCombat
    {
        private readonly Group _playerGroup;
        private readonly Combat _combat;

        private readonly IList<CombatUnit> _unitQueue;
        private readonly IList<CombatUnit> _allUnitList;

        public ActiveCombat(Group playerGroup, Combat combat)
        {
            _playerGroup = playerGroup;
            _combat = combat;

            _unitQueue = new List<CombatUnit>();
            _allUnitList = new List<CombatUnit>();
        }

        public void StartRound()
        {
            _unitQueue.Clear();

            foreach (var unit in _allUnitList)
            {
                _unitQueue.Add(unit);
            }
        }

        public IEnumerable<CombatUnit> Units => _unitQueue.ToArray();

        public CombatUnit? CurrentUnit => _unitQueue.FirstOrDefault();

        internal bool NextUnit()
        {
            _unitQueue.RemoveAt(0);
            return _unitQueue.Count == 0;
        }

        internal void Initialize()
        {
            _allUnitList.Clear();

            foreach (var unit in _playerGroup.Units)
            {
                var combatUnit = new CombatUnit(unit);
                _allUnitList.Add(combatUnit);
            }

            foreach (var unit in _combat.EnemyGroup.Units)
            {
                var combatUnit = new CombatUnit(unit);
                _allUnitList.Add(combatUnit);
            }
        }
    }
}
