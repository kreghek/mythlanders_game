using System;
using System.Collections.Generic;
using System.Linq;

using Rpg.Client.Assets.Perks;

namespace Rpg.Client.Core
{
    internal static class MonsterGeneratorHelper
    {
        public static IReadOnlyList<Unit> CreateMonsters(GlobeNode node, IDice dice, Biome biome, int combatLevel,
            IUnitSchemeCatalog unitSchemeCatalog)
        {
            var availableMonsters = unitSchemeCatalog.AllMonsters
                .Where(x => (!HasPerk<BossMonster>(x, combatLevel)) ||
                            (HasPerk<BossMonster>(x, combatLevel) && !biome.IsComplete &&
                             x.MinRequiredBiomeLevel is not null &&
                             x.MinRequiredBiomeLevel.Value <= biome.Level))
                .Where(x => x.Biome == biome.Type &&
                            ((x.LocationSids is not null && x.LocationSids.Contains(node.Sid)) ||
                             x.LocationSids is null))
                .ToList();

            if (availableMonsters.Any(x => HasPerk<BossMonster>(x, combatLevel)))
            {
                // This location for a boss.
                // Boss has the highest priority so generate only one boss and ignore other units.
                var bossScheme = availableMonsters.Single(x => HasPerk<BossMonster>(x, combatLevel));
                var unit = new Unit(bossScheme, combatLevel);
                return new[] { unit };
            }

            var rolledUnits = new List<UnitScheme>();

            var predefinedMinMonsterCounts = GetPredefinedMonsterCounts(combatLevel);
            var predefinedMinMonsterCount = dice.RollFromList(predefinedMinMonsterCounts, 1).Single();
            var monsterCount = GetMonsterCount(node, biome, availableMonsters, predefinedMinMonsterCount);

            for (var i = 0; i < monsterCount; i++)
            {
                var scheme = dice.RollFromList(availableMonsters, 1).Single();

                var isRegularMonster = !HasPerk<BossMonster>(scheme, combatLevel);

                if (isRegularMonster)
                {
                    rolledUnits.Add(scheme);

                    if (scheme.IsUnique)
                    {
                        // Remove all unique monsters from roll list.
                        availableMonsters.RemoveAll(x => x.IsUnique);
                    }
                }
                else
                {
                    // A big monster is alone on a battleground.
                    rolledUnits.Clear();
                    rolledUnits.Add(scheme);
                    break;
                }
            }

            var units = new List<Unit>();
            foreach (var unitScheme in rolledUnits)
            {
                var unitLevel = GetUnitLevel(combatLevel);
                var unit = new Unit(unitScheme, unitLevel);
                units.Add(unit);
            }

            return units;
        }

        private static int GetMonsterCount(GlobeNode node, Biome biome, List<UnitScheme> availableMonsters,
            int predefinedMinMonsterCount)
        {
            if (node.IsLast && !biome.IsComplete)
            {
                return 1;
            }

            var availableMinMonsterCount = predefinedMinMonsterCount; //Math.Min(predefinedMinMonsterCount, availableMonsters.Count);
            return availableMinMonsterCount;
        }

        private static int[] GetPredefinedMonsterCounts(int combatLevel)
        {
            /*return biomeLevel switch
            {
                0 => new[] { 2 },
                1 => new[] { 2, 2, 3 },
                2 => new[] { 2, 2, 3, 3 },
                > 3 and <= 10 => new[] { 2, 2, 3, 3, 3, 3 },
                > 10 => new[] { 3, 3, 3 },
                _ => new[] { 1, 1, 1, 1, 1, 1, 3, 3, 3, 3, 3 }
            };*/

            return new[] { 3 };
        }

        private static int GetUnitLevel(int combatLevel)
        {
            // +1 because combat starts with zero.
            // But a unit's level have to starts with 1.
            return combatLevel + 1;
        }

        private static bool HasPerk<TPerk>(UnitScheme unitScheme, int combatLevel)
        {
            var unit = new Unit(unitScheme, combatLevel);
            return unit.Perks.OfType<TPerk>().Any();
        }
    }
}