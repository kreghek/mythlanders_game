using System.Collections.Generic;

namespace Rpg.Client.Core
{
    internal class Unit
    {
        public string Name { get; set; }
        public int Hp { get; set; }
        public int MaxHp { get; set; }

        public int Mana { get; set; }
        public int ManaMax { get; set; }

        public IEnumerable<CombatSkill> Skills { get; set; }

        public bool IsPlayerControlled { get; set; }
    }
}
