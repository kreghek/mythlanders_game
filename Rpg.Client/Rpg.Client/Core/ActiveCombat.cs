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

        public ActiveCombat(Group playerGroup, Combat combat, Biom biom)
        {
            _playerGroup = playerGroup;
            _combat = combat;
            Biom = biom;
            _unitQueue = new List<CombatUnit>();
            _allUnitList = new List<CombatUnit>();
        }

        public void StartRound()
        {
            _unitQueue.Clear();

            foreach (var unit in _allUnitList)
            {
                if (!unit.Unit.IsDead)
                {
                    _unitQueue.Add(unit);
                }
            }
        }

        public IEnumerable<CombatUnit> Units => _allUnitList.ToArray();

        public CombatUnit? CurrentUnit => _unitQueue.FirstOrDefault(x => !x.Unit.IsDead);

        internal bool NextUnit()
        {
            _unitQueue.RemoveAt(0);
            return _unitQueue.Count == 0;
        }

        internal bool Finished
        {
            get
            {
                var playerUnits = _allUnitList.Where(x => !x.Unit.IsDead && x.Unit.IsPlayerControlled);
                var hasPlayerUnits = playerUnits.Any();

                var cpuUnits = _allUnitList.Where(x => !x.Unit.IsDead && !x.Unit.IsPlayerControlled);
                var hasCpuUnits = cpuUnits.Any();

                // TODO Looks like XOR
                if (hasPlayerUnits && !hasCpuUnits)
                {
                    return true;
                }
                else if (!hasPlayerUnits && hasCpuUnits)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public Biom Biom { get; }

        internal Combat Combat => _combat;

        internal void Initialize()
        {
            _allUnitList.Clear();

            foreach (var unit in _playerGroup.Units)
            {
                var combatUnit = new CombatUnit(unit);
                _allUnitList.Add(combatUnit);
            }

            foreach (var unit in Combat.EnemyGroup.Units)
            {
                var combatUnit = new CombatUnit(unit);
                _allUnitList.Add(combatUnit);
            }
        }
    }
}