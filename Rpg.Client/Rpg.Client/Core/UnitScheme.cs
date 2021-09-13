using System.Collections.Generic;

namespace Rpg.Client.Core
{
    internal sealed class UnitScheme
    {
        public BiomeType Biom { get; init; }
        public int Hp { get; init; }
        public int HpPerLevel { get; init; }

        public bool IsBoss { get; init; }

        public bool IsUnique { get; init; }
        public string Name { get; init; }

        public IEnumerable<int> NodeIndexes { get; init; }

        public IEnumerable<CombatSkill> Skills { get; init; }
    }
}