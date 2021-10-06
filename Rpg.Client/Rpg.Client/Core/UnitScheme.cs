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

        public int Power { get; set; }

        public int PowerPerLevel { get; set; }

        public UnitSchemeAutoTransition SchemeAudoTransiton { get; set; }

        public IReadOnlyList<SkillSet> SkillSets { get; init; }
    }

    internal enum UnitName
    { 
        Undefined,

        /// <summary>
        /// Used only in the events to describe situations.
        /// </summary>
        Environment,

        Berimir,
        Hawk,
        Rada,
        Maosin,
        Taochan,
        Kakhotep,
        Hq,
        Oldman,
        GuardianWoman,

        Aspid,
        Liho,  // лихо
        Kikimore, // кикимора
        SwampWomanFish,  // болотная русалка
        Drekava, // Дрекавак
        GreyWolf,
        Bear,
        Volkolak,
        Dead, // Умертивие
        Wisp,
        Korgorush,
        Stryga,
        Vampire, // Вурдалак
        HornedFrog, // Рогатая жаба
        Basilisk,
        Taote,
        Sphynx,
        Hydra,
        KosheyTheImmortal,
        KosheyTheImmortal2,
        KosheyTheImmortal3,
    }
}