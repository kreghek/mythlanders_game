using System.Collections.Generic;

using Rpg.Client.Core.Skills;

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

        public int Power { get; set; }

        public int PowerPerLevel { get; set; }

        public IEnumerable<SkillBase> Skills { get; init; }
    }
}