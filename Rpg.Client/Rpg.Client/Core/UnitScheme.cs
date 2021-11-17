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

        public UnitName Name { get; init; }

        public IEnumerable<int> NodeIndexes { get; init; }

        public bool IsMonster { get; init; }

        public float Power => CalcPower();

        private const float HERO_POWER_MULTIPLICATOR = 3f;
        private const float BASE_POWER = 1f;
        
        private float CalcPower()
        {
            if (!IsMonster)
            {
                return BASE_POWER * HERO_POWER_MULTIPLICATOR;
            }

            return BASE_POWER;
            
            /*if (EquipmentLevel > 0)
            {
                return unitScheme.Power + (int)Math.Round(PowerIncrease * (Level * 0.5f + EquipmentLevel * 0.5f),
                    MidpointRounding.AwayFromZero);
            }
            else
            {
                // The monsters do not use equipment level. They has no equipment at all.
                Power = unitScheme.Power + PowerIncrease * Level;
            }*/
        }

        public int PowerPerLevel { get; set; }

        public UnitSchemeAutoTransition SchemeAudoTransiton { get; set; }

        public IReadOnlyList<SkillSet> SkillSets { get; init; }

        public UnitGraphicsConfigBase UnitGraphicsConfig { get; init; }
    }
}