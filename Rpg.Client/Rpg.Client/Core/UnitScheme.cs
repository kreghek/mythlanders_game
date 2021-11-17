using System.Collections.Generic;

namespace Rpg.Client.Core
{
    internal sealed class UnitScheme
    {
        public BiomeType Biome { get; init; }
        public int Hp { get; init; }
        public int HpPerLevel { get; init; }

        public bool IsBoss { get; init; }

        public bool IsUnique { get; init; }

        public UnitName Name { get; init; }

        public IEnumerable<int>? NodeIndexes { get; init; }

        public bool IsMonster { get; init; }

        public float Power => CalcPower();

        private const float HERO_POWER_MULTIPLICATOR = 3f;
        private const float BASE_POWER = 1f;
        private const float POWER_PER_LEVEL = 0.1f;
        
        private float CalcPower()
        {
            if (!IsMonster)
            {
                return BASE_POWER * HERO_POWER_MULTIPLICATOR;
            }

            return BASE_POWER;
        }

        public float PowerPerLevel => CalcPowerPerLevel();

        private float CalcPowerPerLevel()
        {
            if (!IsMonster)
            {
                return POWER_PER_LEVEL * HERO_POWER_MULTIPLICATOR;
            }

            return POWER_PER_LEVEL;
        }

        public UnitSchemeAutoTransition? SchemeAutoTransition { get; init; }

        public IReadOnlyList<SkillSet> SkillSets { get; init; }

        public UnitGraphicsConfigBase UnitGraphicsConfig { get; init; }
    }
}