using System.Collections.Generic;
using System.Linq;

namespace Rpg.Client.Core
{
    internal class ActiveCombat
    {
        private readonly IList<CombatUnit> _allUnitList;
        private readonly Group _playerGroup;

        private readonly IList<CombatUnit> _unitQueue;

        public ActiveCombat(Group playerGroup, Combat combat, Biome biom)
        {
            _playerGroup = playerGroup;
            Combat = combat;
            Biom = biom;
            _unitQueue = new List<CombatUnit>();
            _allUnitList = new List<CombatUnit>();
        }

        public Biome Biom { get; }

        public CombatUnit? CurrentUnit => _unitQueue.FirstOrDefault(x => !x.Unit.IsDead);

        public IEnumerable<CombatUnit> Units => _allUnitList.ToArray();

        internal Combat Combat { get; }

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

                if (!hasPlayerUnits && hasCpuUnits)
                {
                    return true;
                }

                return false;
            }
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

        internal bool NextUnit()
        {
            _unitQueue.RemoveAt(0);
            return _unitQueue.Count == 0;
        }
    }
}